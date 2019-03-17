namespace Roton.Composers.Video.Glyphs.Impl
{
    public sealed class BitmapFont : IBitmapFont
    {
        public BitmapFont(byte[] data, int width, int height)
        {
            Data = data;
            Height = height;
            Width = width;
        }

        public byte[] Data { get; }
        public int Height { get; }
        public int Width { get; }
    }
}
