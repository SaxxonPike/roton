using System.Collections.Generic;

namespace Roton.Interface.Video.Glyphs
{
    public interface IComposedGlyph
    {
        int Index { get; }
        int Width { get; }
        int Height { get; }
        IEnumerable<int> Data { get; }
    }
}
