using System.Collections.Generic;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class Glyph(int index, int width, int height, IEnumerable<int> data)
    : IGlyph
{
    public int Index { get; } = index;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public IReadOnlyList<int> Data { get; } = [.. data];
}