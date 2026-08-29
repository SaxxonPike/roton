using System;
using Roton.Emulation.Core;

namespace Roton.Composers.Audio.AudioStreams;

/// <summary>
/// Handles synthesizing the PC speaker.
/// </summary>
public interface IAudioStreamComposer : ISpeaker
{
    /// <summary>
    /// Raised when there is an audio buffer ready.
    /// </summary>
    event EventHandler<AudioStreamDataEventArgs> BufferReady;
    
    /// <summary>
    /// Sample rate that will be used by the composer in Hz.
    /// </summary>
    int SampleRate { get; set; }
}