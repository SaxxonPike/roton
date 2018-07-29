namespace Roton.Composers.Video.Glyphs
{
    public interface IBitmapFont
    {
        byte[] Data { get; }
        int Height { get; }
        int Width { get; }
    }
}