using System;
using System.Collections.Generic;
using Roton.Composers.Audio.AudioStreams;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Speaker(
    IEnumerable<IAudioStreamComposer> composers,
    IDrumSoundList drumSoundList,
    IConfig config)
    : ISpeaker
{
    private readonly List<IAudioStreamComposer> _composers = [.. composers];

    /// <summary>
    /// Reference frequency.
    /// </summary>
    private const double RootFrequency = 440d;

    /// <summary>
    /// Root note corresponding to the reference frequency.
    /// </summary>
    private const int RootNote = 45;

    /// <summary>
    /// Duration of the step sound effect, in seconds.
    /// </summary>
    private const float StepDuration = 1f / 22050f;

    public void PlayDrum(int drum)
    {
        var duration = config.Audio.DrumDuration / 44100f;
        var drumSound = drumSoundList[drum];
        var tones = (stackalloc SpeakerTone[drumSound.Length + 1]);
        tones[drumSound.Length] = default;

        for (var i = 0; i < drumSound.Length; i++)
            tones[i] = new SpeakerTone(drumSound[i], duration);

        foreach (var composer in _composers)
            composer.PlayToneSequence(tones);
    }

    public void PlayNote(int note)
    {
        foreach (var composer in _composers)
            composer.PlayTone((float)(RootFrequency * Math.Pow(2d, (note - RootNote) / 12d)));
    }

    public void PlayStep()
    {
        var tones = (stackalloc SpeakerTone[2]);
        tones[0] = new SpeakerTone(1, StepDuration);
        foreach (var composer in _composers)
            composer.PlayToneSequence(tones);
    }

    public void StopNote()
    {
        foreach (var composer in _composers)
            composer.StopTone();
    }
}