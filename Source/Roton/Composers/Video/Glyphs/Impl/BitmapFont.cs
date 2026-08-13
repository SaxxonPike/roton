namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class BitmapFont(byte[] data, int width, int height) : IBitmapFont
{
    public byte[] Data { get; } = data;
    public int Height { get; } = height;
    public int Width { get; } = width;
}