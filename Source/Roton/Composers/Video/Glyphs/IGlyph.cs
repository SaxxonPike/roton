using System.Collections.Generic;

namespace Roton.Composers.Video.Glyphs;

public interface IGlyph
{
    int Index { get; }
    int Width { get; }
    int Height { get; }
    IReadOnlyList<int> Data { get; }
}