using System;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class BitmapFont(ReadOnlyMemory<byte> data, int width, int height)
    : IBitmapFont
{
    public ReadOnlyMemory<byte> Data => data;
    public int Height => height;
    public int Width => width;
}