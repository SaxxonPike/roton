namespace Roton.Emulation.Data.Impl;

public sealed class Sound(params int[] data) : ISound
{
    public int this[int index] => data[index];
    public int Length => data.Length;
}