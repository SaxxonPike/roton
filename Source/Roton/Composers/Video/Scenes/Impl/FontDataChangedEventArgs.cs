using System;

namespace Roton.Composers.Video.Scenes.Impl;

public sealed class FontDataChangedEventArgs : EventArgs
{
    public byte[] Data { get; set; }
}