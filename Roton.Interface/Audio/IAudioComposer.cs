using System;
using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Interface.Events;

namespace Roton.Interface.Audio
{
    public interface IAudioComposer : ISpeaker
    {
        event EventHandler<AudioComposerDataEventArgs> BufferReady;
        int SampleRate { get; }
    }
}
