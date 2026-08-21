using System;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class Glyph(int index, int width, int height, ReadOnlySpan<int> data)
    : IGlyph
{
    private readonly int[] _data = [.. data];
    
    public int Index { get; } = index;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public ReadOnlySpan<int> Data => _data;
}