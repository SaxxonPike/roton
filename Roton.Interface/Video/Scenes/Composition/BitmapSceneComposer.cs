using System.Linq;
using Roton.Core;
using Roton.Interface.Extensions;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;

namespace Roton.Interface.Video.Scenes.Composition
{
    public class BitmapSceneComposer : SceneComposer, IBitmapSceneComposer
    {
        private readonly IGlyphComposer _glyphComposer;
        private int _stride;
        private int[] _offsetLookUpTable;
        private readonly int[] _colors;
        private bool _hideBlinkingCharacters;
        private bool _useFullBrightBackgrounds;

        public BitmapSceneComposer(
            IGlyphComposer glyphComposer, 
            IPaletteComposer paletteComposer,
            int columns, 
            int rows) : base(columns, rows)
        {
            _glyphComposer = new CachedGlyphComposer(glyphComposer);
            _colors = paletteComposer.ComposeAllColors().Select(c => c.ToArgb()).ToArray();
            InitializeNewBitmap();
        }

        public override void SetSize(int width, int height, bool wide)
        {
            base.SetSize(width, height, wide);
            InitializeNewBitmap();
        }

        private void InitializeNewBitmap()
        {
            if (_glyphComposer == null)
                return;

            var charTotal = Columns * Rows;
            _stride = Columns * _glyphComposer.MaxWidth;
            _offsetLookUpTable = Enumerable.Range(0, charTotal)
                .Select(i => _glyphComposer.MaxWidth * (i % Columns) + _glyphComposer.MaxHeight * _stride * (i / Columns))
                .ToArray();
            DirectAccessBitmap?.Dispose();
            DirectAccessBitmap = new DirectAccessBitmap(_stride, Rows * _glyphComposer.MaxHeight);
        }

        protected sealed override void OnGlyphUpdated(int index, AnsiChar ac)
        {
            base.OnGlyphUpdated(index, ac);
            DrawGlyph(ac, _offsetLookUpTable[index]);
        }

        private void DrawGlyph(AnsiChar ac, int offset)
        {
            var glyph = _glyphComposer.ComposeGlyph(ac.Char);
            var inputBits = glyph.Data;
            var outputBits = DirectAccessBitmap.Bits;
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
        }

        public IDirectAccessBitmap DirectAccessBitmap { get; private set; }

        public void Dispose()
        {
            DirectAccessBitmap?.Dispose();
        }

        private void UpdateAllBlinkingCharacters()
        {
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Columns; x++)
                {
                    var index = GetBufferOffset(x, y);
                    var c = Chars[index];
                    if ((c.Color & 0x80) != 0)
                        OnGlyphUpdated(index, Chars[index]);
                }
            }
        }

        public bool HideBlinkingCharacters
        {
            get { return _hideBlinkingCharacters; }
            set
            {
                _hideBlinkingCharacters = value;
                if (!UseFullBrightBackgrounds)
                    UpdateAllBlinkingCharacters();
            }
        }

        public bool UseFullBrightBackgrounds
        {
            get { return _useFullBrightBackgrounds; }
            set
            {
                _useFullBrightBackgrounds = value;
                UpdateAllBlinkingCharacters();
            }
        }
    }
}
