namespace Roton.Composers.Video.Glyphs
{
    public interface IGlyphComposerFactory
    {
        IGlyphComposer Get(byte[] data, bool wide);
    }
}