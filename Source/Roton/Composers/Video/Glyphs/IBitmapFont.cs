using System;

namespace Roton.Composers.Video.Glyphs;

public interface IBitmapFont
{
    ReadOnlyMemory<byte> Data { get; }
    int Height { get; }
    int Width { get; }
}