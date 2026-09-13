using System;

namespace Roton.Composers.Audio;

/// <summary>
/// Handles synthesizing the PC speaker.
/// </summary>
public interface IAudioStreamComposer
{
    void PlayToneSequence(ReadOnlySpan<SpeakerTone> tones);
    void PlayTone(float frequency);
    void StopTone();
    void Tick();

    /// <summary>
    /// Raised when there is an audio buffer ready.
    /// </summary>
    event EventHandler<AudioStreamDataEventArgs> BufferReady;

    /// <summary>
    /// Sample rate that will be used by the composer in Hz.
    /// </summary>
    int SampleRate { get; set; }
}