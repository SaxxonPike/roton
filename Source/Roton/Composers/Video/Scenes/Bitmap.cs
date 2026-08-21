using System;

namespace Roton.Composers.Video.Scenes;

public sealed class Bitmap(int width, int height)
{
    private readonly int[] _bits = new int[width * height];

    public Span<int> Bits => _bits;
    public int Height { get; } = height;
    public int Width { get; } = width;
    public int Stride => Width * sizeof(int);
}