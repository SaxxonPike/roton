using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Startup)]
internal sealed class SoundUnit(
    IState state,
    ISpeaker speaker,
    IMusicEncoder musicEncoder)
    : ISoundUnit
{
    public void PlaySound(int priority, ISound sound, int? offset = null, int? length = null)
    {
        if (state.GameOver || state.GameQuiet)
            return;

        var soundIsNotPlaying = !state.SoundPlaying;
        var soundIsMusic = priority == -1;
        var soundIsHigherPriority = state.SoundPriority != -1 && priority >= state.SoundPriority;

        if (!(soundIsNotPlaying || soundIsMusic || soundIsHigherPriority))
            return;

        if (!soundIsMusic || state.SoundPriority != -1)
            state.SoundBuffer.Clear();

        Console.WriteLine($"enqueue sound priority={priority} len={sound.Length}");
        state.SoundBuffer.Enqueue(sound, offset, length);
        state.SoundPlaying = true;
        state.SoundPriority = priority;
    }

    public void ClearSound()
    {
        state.SoundPlaying = false;
        speaker.StopNote();
    }

    public void PlayStep()
    {
        if (state.GameOver || state.GameQuiet || state.SoundPlaying)
            return;

        speaker.PlayStep();
    }

    public void PlayErrorSound()
    {
        ClearSound();
        PlaySound(1, musicEncoder.Encode("s004x114x9"));
    }
}