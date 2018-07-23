namespace Roton.Composers.Video.Glyphs
{
    public class BitmapFont
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
