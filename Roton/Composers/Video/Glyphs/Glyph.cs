using System.Collections.Generic;
using System.Linq;

namespace Roton.Composers.Video.Glyphs
{
    public class Glyph : IGlyph
    {
        public Glyph(int index, int width, int height, IEnumerable<int> data)
        {
            Index = index;
            Width = width;
            Height = height;
            Data = data.ToArray();
        }

        public int Index { get; }
        public int Width { get; }
        public int Height { get; }
        public IList<int> Data { get; }
    }
}