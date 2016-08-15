using System;
using System.Collections.Generic;

namespace Roton.Interface.Video.Glyphs.Composers
{
    public class VgaGlyphComposer : IGlyphComposer
    {
        private class Glyph : IComposedGlyph
        {
            public Glyph(int index, int width, int height, IEnumerable<int> data)
            {
                Index = index;
                Width = width;
                Height = height;
                Data = data;
            }

            public int Index { get; }
            public int Width { get; }
            public int Height { get; }
            public IEnumerable<int> Data { get; }
        }

        private readonly byte[] _data;
        private readonly int _onValue;
        private readonly int _offValue;
        private readonly int _height;

        public VgaGlyphComposer(byte[] data) : this(data, -1, 0)
        {
        }

        public VgaGlyphComposer(byte[] data, int onValue, int offValue)
        {
            _data = data;
            _onValue = onValue;
            _offValue = offValue;
            _height = data.Length >> 8;
        }

        public IComposedGlyph ComposeGlyph(int index)
        {
            var output = new int[_height*8];
            var outputOffset = 0;
            var inputOffset = (index & 0xFF)*_height;
            for (var y = 0; y < _height; y++)
            {
                var bits = _data[inputOffset++];
                for (var x = 0; x < 8; x++)
                {
                    output[outputOffset++] = (bits & 0x80) != 0 ? _onValue : _offValue;
                    bits <<= 1;
                }
            }
            return new Glyph(index, 8, _height, output);
        }
    }
}