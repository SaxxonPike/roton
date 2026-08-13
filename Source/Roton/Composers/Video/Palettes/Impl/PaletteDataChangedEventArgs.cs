using System;

namespace Roton.Composers.Video.Palettes.Impl;

public sealed class PaletteDataChangedEventArgs : EventArgs
{
    public byte[] Data { get; set; }
}