using System;

namespace Roton.Composers.Audio.Impl;

public class AudioComposerDataEventArgs : EventArgs
{
    public float[] Data { get; set; }
}