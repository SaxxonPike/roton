using System.Linq;
using Roton.Core;
using Roton.Interface.Extensions;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;

namespace Roton.Interface.Video.Scenes.Composition
{
    public class BitmapSceneComposer : ISceneComposer
    {
        private readonly IGlyphComposer _glyphComposer;
        private readonly int _columns;
        private readonly int _rows;
        private readonly AnsiChar[] _chars;
        private readonly int _stride;
        private readonly int[] _offsetLookUpTable;
        private readonly AnsiChar _blankCharacter;
        private readonly IFastBitmap _bitmap;
        private readonly int[] _colors;

        public BitmapSceneComposer(
            IGlyphComposer glyphComposer, 
            IPaletteComposer paletteComposer,
            int columns, 
            int rows, 
            int glyphMaxWidth,
            int glyphMaxHeight,
            int scaleX, 
            int scaleY)
        {
            var charTotal = _columns * _rows;
            _columns = columns;
            _rows = rows;

            var scaledGlyphComposer = new ScaledGlyphComposer(glyphComposer, scaleX, scaleY);
            _glyphComposer = new CachedGlyphComposer(scaledGlyphComposer);
            _blankCharacter = new AnsiChar();

            var glyphWidth = glyphMaxWidth*scaleX;
            var glyphHeight = glyphMaxHeight*scaleY;
            _chars = new AnsiChar[charTotal];
            _stride = _columns*glyphWidth;
            _offsetLookUpTable = Enumerable.Range(0, charTotal)
                .Select(i => glyphWidth * (i % _columns) + glyphHeight * (i / _columns))
                .ToArray();
            _bitmap = new FastBitmap(_stride, _rows * glyphHeight);
            _colors = paletteComposer.ComposeAllColors().Select(c => c.ToArgb()).ToArray();
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return (x < 0 || x >= _columns || y < 0 || y >= _rows);
        }

        private int GetBufferOffset(int x, int y)
        {
            return x + y*_columns;
        }

        public AnsiChar GetChar(int x, int y)
        {
            return IsOutOfBounds(x, y)
                ? _blankCharacter
                : _chars[GetBufferOffset(x, y)];
        }

        public void SetChar(int x, int y, AnsiChar ac)
        {
            if (IsOutOfBounds(x, y))
                return;

            var index = GetBufferOffset(x, y);
            _chars[index] = ac;
            DrawGlyph(ac, _offsetLookUpTable[index]);
        }

        private void DrawGlyph(AnsiChar ac, int offset)
        {
            var glyph = _glyphComposer.ComposeGlyph(ac.Char);
            var inputBits = glyph.Data;
            var outputBits = _bitmap.Bits;
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
    }
}
