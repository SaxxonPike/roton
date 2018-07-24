using System;
using Roton.Composers.Audio.Impl;
using Roton.Emulation.Core;

namespace Roton.Composers.Audio
{
    public interface IAudioComposer : ISpeaker
    {
        event EventHandler<AudioComposerDataEventArgs> BufferReady;
        int SampleRate { get; }
    }
}
