using System;

namespace Roton.Composers.Video.Scenes;

public readonly struct FontDataChangedEventArgs(ReadOnlyMemory<byte> data)
{
    public ReadOnlyMemory<byte> Data => data;
}