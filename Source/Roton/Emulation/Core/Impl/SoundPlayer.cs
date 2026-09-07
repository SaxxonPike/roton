using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class SoundPlayer(
    IState state,
    ISpeaker speaker,
    IMusicEncoder musicEncoder)
    : ISoundPlayer
{
    public void PlaySound(int priority, ReadOnlySpan<byte> sound)
    {
        if (state.GameOver || state.GameQuiet)
            return;

        var soundIsNotPlaying = !state.SoundPlaying;
        var soundIsMusic = priority == -1;
        var soundIsHigherPriority = state.SoundPriority != -1 && priority >= state.SoundPriority;

        if (!(soundIsNotPlaying || soundIsMusic || soundIsHigherPriority))
            return;

        if (!soundIsMusic)
            state.SoundBuffer.Clear();

        state.SoundBuffer.Enqueue(sound);
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
        using var mem = musicEncoder.Encode("s004x114x9");
        PlaySound(1, mem.Span);
    }

    public void UpdateSound()
    {
        if (!state.SoundPlaying)
        {
            state.SoundBuffer.Clear();
            return;
        }

        if (state.SoundTicks <= 0)
        {
            if (state.SoundBuffer.Count > 0)
            {
                var sound = state.SoundBuffer.Dequeue();
                state.SoundTicks = sound.Duration << 2;
                switch (sound.Note)
                {
                    case >= 0xF0:
                    {
                        speaker.PlayDrum(sound.Note - 0xF0);
                        break;
                    }
                    case > 0x00:
                    {
                        var actualNote = (sound.Note & 0xF) + (sound.Note >> 4) * 12;
                        speaker.PlayNote(actualNote);
                        break;
                    }
                    default:
                    {
                        speaker.StopNote();
                        break;
                    }
                }
            }
            else
            {
                state.SoundPlaying = false;
                state.SoundPriority = 0;
                speaker.StopNote();
            }
        }

        if (state.SoundPlaying)
            state.SoundTicks--;
    }
}