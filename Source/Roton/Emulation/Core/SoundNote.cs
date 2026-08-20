namespace Roton.Emulation.Core;

public readonly struct SoundNote(int note, int duration)
{
    public int Note { get; } = note;
    public int Duration { get; } = duration;
}