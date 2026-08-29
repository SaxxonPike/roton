using System;

namespace Roton.Composers.Audio.AudioStreams.Impl;

/// <summary>
/// Implements a null audio composer.
/// </summary>
/// <remarks>
/// No audio will be generated.
/// </remarks>
public class NullAudioStreamComposer : IAudioStreamComposer
{
    public event EventHandler<AudioStreamDataEventArgs>? BufferReady;

    public void PlayDrum(int drum)
    {
    }

    public void PlayNote(int note)
    {
    }

    public void PlayStep()
    {
    }

    public void Tick()
    {
    }

    public void StopNote()
    {
    }

    public int SampleRate { get; set; }
}