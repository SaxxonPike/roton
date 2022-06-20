using System;

namespace Roton.Composers.Video.Palettes.Impl;

public class PaletteDataChangedEventArgs : EventArgs
{
    public byte[] Data { get; set; }
}