using System;

namespace Roton.Composers.Video.Scenes.Impl;

public readonly struct FontDataChangedEventArgs(byte[]? data)
{
    public byte[]? Data => data;
}