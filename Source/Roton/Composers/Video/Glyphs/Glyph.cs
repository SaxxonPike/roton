using System;

namespace Roton.Composers.Video.Glyphs;

public sealed class Glyph(int width, int height, ReadOnlyMemory<int> data)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public ReadOnlyMemory<int> Data => data;
}