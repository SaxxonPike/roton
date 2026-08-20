using System;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class BitmapFont(ReadOnlySpan<byte> data, int width, int height) : IBitmapFont
{
    public ReadOnlyMemory<byte> Data { get; } = data.ToArray();
    public int Height { get; } = height;
    public int Width { get; } = width;
}