using System;

namespace Roton.Composers.Audio.Impl;

public sealed class AudioComposerDataEventArgs : EventArgs
{
    public float[] Data { get; set; }
}