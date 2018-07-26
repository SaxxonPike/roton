using System;
using System.Linq;
using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Composers.Video.Palettes.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Composers.Video.Scenes.Impl
{
    public sealed class SceneComposer : ISceneComposer, IDisposable
    {
        public event EventHandler<FontDataChangedEventArgs> FontDataChanged;
        public event EventHandler<PaletteDataChangedEventArgs> PaletteDataChanged;
        public event EventHandler<ResizedEventArgs> Resized;
        public event EventHandler<SceneUpdatedEventArgs> SceneUpdated;
        
        private readonly AnsiChar _blankCharacter;
        private readonly IGlyphComposerFactory _glyphComposerFactory;
        private readonly IPaletteComposerFactory _paletteComposerFactory;
        private int[] _colors;

        private byte[] _fontData;
        private IGlyphComposer _glyphComposer;
        private bool _hideBlinkingCharacters;
        private int[] _offsetLookUpTable;
        private IPaletteComposer _paletteComposer;
        private byte[] _paletteData;

        private int _stride;
        private bool _useFullBrightBackgrounds;

        private AnsiChar[] _chars;

        public SceneComposer(
            IPaletteComposerFactory paletteComposerFactory,
            IGlyphComposerFactory glyphComposerFactory
        )
        {
            _blankCharacter = new AnsiChar();
            _paletteComposerFactory = paletteComposerFactory;
            _glyphComposerFactory = glyphComposerFactory;

            InitializePalette();
            InitializeFont();
            InitializeNewBitmap();
        }

        public IBitmap Bitmap { get; private set; }

        public bool HideBlinkingCharacters
        {
            get => _hideBlinkingCharacters;
            set
            {
                _hideBlinkingCharacters = value;
                if (!UseFullBrightBackgrounds)
                    UpdateAllBlinkingCharacters();
            }
        }

        public bool UseFullBrightBackgrounds
        {
            get => _useFullBrightBackgrounds;
            set
            {
                _useFullBrightBackgrounds = value;
                UpdateAllBlinkingCharacters();
            }
        }

        public bool Wide { get; private set; }

        public int Columns { get; private set; }

        public int Rows { get; private set; }

        public void Update(int x, int y)
        {
            var index = GetBufferOffset(x, y);
            Update(index, _chars[index]);
        }

        public void Clear()
        {
            for (var y = 0; y < Rows; y++)
            for (var x = 0; x < Columns; x++)
                Plot(x, y, _blankCharacter);
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            if (IsOutOfBounds(x, y))
                return;

            var index = GetBufferOffset(x, y);
            _chars[index] = ac;
            Update(index, ac);
        }

        public AnsiChar Read(int x, int y)
        {
            return IsOutOfBounds(x, y)
                ? _blankCharacter
                : _chars[GetBufferOffset(x, y)];
        }

        public void SetFont(byte[] data)
        {
            _fontData = data.ToArray();
            InitializeFont();
        }

        public void SetPalette(byte[] data)
        {
            _paletteData = data.ToArray();
            InitializePalette();
        }

        public void SetSize(int width, int height, bool wide)
        {
            Rows = height;
            Columns = width;
            Wide = wide;

            var charTotal = Columns * Rows;
            _chars = new AnsiChar[charTotal];

            InitializeFont();
            InitializeNewBitmap();

            Resized?.Invoke(this, new ResizedEventArgs {Width = width, Height = height, Wide = wide});
        }

        public void Write(int x, int y, string value, int color)
        {
            foreach (var b in (value ?? string.Empty).ToBytes())
            {
                if (y >= Rows)
                    break;

                while (x >= Columns)
                {
                    x -= Columns;
                    y++;
                }

                Plot(x++, y, new AnsiChar(b, color));
            }
        }

        public void Dispose()
        {
            Bitmap?.Dispose();
        }

        private void DrawGlyph(AnsiChar ac, int offset)
        {
            var glyph = _glyphComposer.ComposeGlyph(ac.Char);
            var inputBits = glyph.Data;
            var outputBits = Bitmap.Bits;
            var width = glyph.Width;
            var height = glyph.Height;
            var baseOffset = offset;
            var inputOffset = 0;
            var backgroundColor = _useFullBrightBackgrounds
                ? _colors[ac.Color >> 4]
                : _colors[(ac.Color >> 4) & 0x7];
            var foregroundColor = !_hideBlinkingCharacters || (ac.Color & 0x80) == 0
                ? _colors[ac.Color & 0x0F]
                : backgroundColor;
            for (var y = 0; y < height; y++)
            {
                var outputOffset = baseOffset;
                for (var x = 0; x < width; x++)
                {
                    var inputBitData = inputBits[inputOffset++];
                    outputBits[outputOffset++] = (inputBitData & foregroundColor) | (~inputBitData & backgroundColor);
                }

                baseOffset += _stride;
            }
            
            SceneUpdated?.Invoke(this, new SceneUpdatedEventArgs());
        }

        private int GetBufferOffset(int x, int y)
        {
            return x + y * Columns;
        }

        private void InitializeNewBitmap()
        {
            if (_glyphComposer == null)
                return;

            var charTotal = Columns * Rows;
            var stride = Columns * _glyphComposer.MaxWidth;
            var height = Rows * _glyphComposer.MaxHeight;
            
            if (Bitmap != null)
            {
                if (Bitmap.Height == height && Bitmap.Width == stride)
                    return;
            }

            var offsetLookUpTable = Enumerable.Range(0, charTotal)
                .Select(i =>
                    _glyphComposer.MaxWidth * (i % Columns) + _glyphComposer.MaxHeight * stride * (i / Columns))
                .ToArray();

            var oldBitmap = Bitmap;
            _stride = stride;
            _offsetLookUpTable = offsetLookUpTable;
            Bitmap = new Bitmap(stride, height);
            oldBitmap?.Dispose();
        }

        private void InitializeFont()
        {
            var oldGlyphComposer = _glyphComposer;
            _glyphComposer = _glyphComposerFactory.Get(_fontData, Wide);
            
            if (oldGlyphComposer != null)
            {
                if (_glyphComposer.MaxHeight != oldGlyphComposer.MaxHeight || _glyphComposer.MaxWidth != oldGlyphComposer.MaxWidth)
                    InitializeNewBitmap();
            }
            
            FontDataChanged?.Invoke(this, new FontDataChangedEventArgs
            {
                Data = _fontData?.ToArray()
            });
        }

        private void InitializePalette()
        {
            _paletteComposer = _paletteComposerFactory.Get(_paletteData);
            _colors = Enumerable
                .Range(0, 16)
                .Select(i => _paletteComposer.ComposeColor(i).ToArgb())
                .ToArray();
            
            PaletteDataChanged?.Invoke(this, new PaletteDataChangedEventArgs
            {
                Data = _paletteData?.ToArray()
            });
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= Columns || y < 0 || y >= Rows;
        }

        private void Update(int index, AnsiChar ac)
        {
            DrawGlyph(ac, _offsetLookUpTable[index]);
        }

        private void UpdateAllBlinkingCharacters()
        {
            for (var y = 0; y < Rows; y++)
            for (var x = 0; x < Columns; x++)
            {
                var index = GetBufferOffset(x, y);
                var c = _chars[index];
                if ((c.Color & 0x80) != 0)
                    Update(index, _chars[index]);
            }
        }
    }
}