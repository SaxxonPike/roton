using System;
using System.Linq;
using Roton.Core;
using Roton.Interface.Extensions;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;

namespace Roton.Interface.Video.Scenes.Composition
{
    public class BitmapSceneComposer : ISceneComposer, IDisposable
    {
        private readonly IGlyphComposer _glyphComposer;
        private readonly AnsiChar[] _chars;
        private readonly int _stride;
        private readonly int[] _offsetLookUpTable;
        private readonly AnsiChar _blankCharacter;
        private readonly int[] _colors;

        public BitmapSceneComposer(
            IGlyphComposer glyphComposer, 
            IPaletteComposer paletteComposer,
            int columns, 
            int rows)
        {
            Columns = columns;
            Rows = rows;
            var charTotal = Columns * Rows;

            _glyphComposer = new CachedGlyphComposer(glyphComposer);
            _blankCharacter = new AnsiChar();

            _chars = new AnsiChar[charTotal];
            _stride = Columns * glyphComposer.MaxWidth;
            _offsetLookUpTable = Enumerable.Range(0, charTotal)
                .Select(i => glyphComposer.MaxWidth * (i % Columns) + glyphComposer.MaxHeight * _stride * (i / Columns))
                .ToArray();
            Bitmap = new FastBitmap(_stride, Rows * glyphComposer.MaxHeight);
            _colors = paletteComposer.ComposeAllColors().Select(c => c.ToArgb()).ToArray();
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return (x < 0 || x >= Columns || y < 0 || y >= Rows);
        }

        private int GetBufferOffset(int x, int y)
        {
            return x + y* Columns;
        }

        public AnsiChar GetChar(int x, int y)
        {
            return IsOutOfBounds(x, y)
                ? _blankCharacter
                : _chars[GetBufferOffset(x, y)];
        }

        public int Rows { get; }

        public void SetChar(int x, int y, AnsiChar ac)
        {
            if (IsOutOfBounds(x, y))
                return;

            var index = GetBufferOffset(x, y);
            _chars[index] = ac;
            DrawGlyph(ac, _offsetLookUpTable[index]);
        }

        public int Columns { get; }

        private void DrawGlyph(AnsiChar ac, int offset)
        {
            var glyph = _glyphComposer.ComposeGlyph(ac.Char);
            var inputBits = glyph.Data;
            var outputBits = Bitmap.Bits;
            var width = glyph.Width;
            var height = glyph.Height;
            var baseOffset = offset;
            var inputOffset = 0;
            var foregroundColor = _colors[ac.Color & 0x0F];
            var backgroundColor = _colors[ac.Color >> 4];

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

        public IFastBitmap Bitmap { get; }

        public void Dispose()
        {
            Bitmap?.Dispose();
        }
    }
}
