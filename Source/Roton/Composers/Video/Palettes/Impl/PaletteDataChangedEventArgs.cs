using System;

namespace Roton.Composers.Video.Palettes.Impl;

public readonly struct PaletteDataChangedEventArgs(byte[] data)
{
    public byte[] Data => data;
}