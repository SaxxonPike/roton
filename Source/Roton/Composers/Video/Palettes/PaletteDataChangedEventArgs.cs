using System;

namespace Roton.Composers.Video.Palettes;

public readonly struct PaletteDataChangedEventArgs(ReadOnlyMemory<byte> data)
{
    public ReadOnlyMemory<byte> Data => data;
}