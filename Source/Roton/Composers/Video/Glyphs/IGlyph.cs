using System;

namespace Roton.Composers.Video.Glyphs;

public interface IGlyph
{
    int Index { get; }
    int Width { get; }
    int Height { get; }
    ReadOnlySpan<int> Data { get; }
}