namespace Roton.Composers.Video.Palettes
{
    public interface IPaletteComposerFactory
    {
        IPaletteComposer Get(byte[] data);
    }
}