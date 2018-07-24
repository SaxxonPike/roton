namespace Roton.Composers.Video.Glyphs
{
    public interface IGlyphComposerFactory
    {
        IGlyphComposer Get(byte[] data, int scaleX, int scaleY);
    }
}