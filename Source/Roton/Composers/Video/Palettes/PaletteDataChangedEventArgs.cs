namespace Roton.Composers.Video.Palettes;

public readonly struct PaletteDataChangedEventArgs(byte[] data)
{
    public byte[] Data => data;
}