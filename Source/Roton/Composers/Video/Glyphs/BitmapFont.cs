using System;

namespace Roton.Composers.Video.Glyphs;

public sealed class BitmapFont(ReadOnlyMemory<byte> data, int width, int height)
{
    public ReadOnlyMemory<byte> Data => data;
    public int Height => height;
    public int Width => width;
}