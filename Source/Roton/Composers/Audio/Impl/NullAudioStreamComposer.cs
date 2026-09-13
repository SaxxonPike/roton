using System;

namespace Roton.Composers.Audio.Impl;

/// <summary>
/// Implements a null audio composer.
/// </summary>
/// <remarks>
/// No audio will be generated.
/// </remarks>
public class NullAudioStreamComposer : IAudioStreamComposer
{
    public event EventHandler<AudioStreamDataEventArgs>? BufferReady;

    public void PlayToneSequence(ReadOnlySpan<SpeakerTone> tones)
    {
    }

    public void PlayTone(float frequency)
    {
    }

    public void StopTone()
    {
    }

    public void Tick()
    {
    }

    public int SampleRate { get; set; }
}