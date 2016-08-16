using System.Collections.Generic;

namespace Roton.Interface.Video.Glyphs
{
    public interface IGlyph
    {
        int Index { get; }
        int Width { get; }
        int Height { get; }
        IList<int> Data { get; }
    }
}
