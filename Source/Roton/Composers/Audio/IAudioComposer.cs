using System;
using Roton.Emulation.Core;

namespace Roton.Composers.Audio;

/// <summary>
/// Handles synthesizing the PC speaker.
/// </summary>
public interface IAudioComposer : ISpeaker
{
    /// <summary>
    /// Raised when there is an audio buffer ready.
    /// </summary>
    event EventHandler<AudioComposerDataEventArgs> BufferReady;
    
    /// <summary>
    /// Sample rate that will be used by the composer in Hz.
    /// </summary>
    int SampleRate { get; set; }
}