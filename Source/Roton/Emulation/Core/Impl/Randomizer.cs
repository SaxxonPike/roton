using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Randomizer(
    IConfig config)
    : IRandomizer
{
    private const int Coefficient = 0x08088405;

    private int GetInitialState(DateTime now)
    {
        if (config.RandomSeed is { } seed)
            return seed;

        seed = (now.Second << 24) |
               ((now.Millisecond / 10) << 16) |
               (now.Hour << 8) |
               now.Minute;

        return seed;
    }

    public void Reset() => 
        State = 1;

    public void SetSeed(DateTime now) => 
        State = GetInitialState(now);

    public int GetNext(int exclusiveUpperBound)
    {
        if (exclusiveUpperBound == 0)
            return 0;

        var result = unchecked((ushort)(State >> 16)) % exclusiveUpperBound;
        State = unchecked(State * Coefficient + 1);
        return result;
    }

    public void GetNext(int exclusiveUpperBound, Span<int> buffer)
    {
        for (var i = 0; i < buffer.Length; i++)
        {
            if (exclusiveUpperBound == 0)
                buffer[i] = 0;
            else
                buffer[i] = unchecked((ushort)(State >> 16)) % exclusiveUpperBound;

            State = unchecked(State * Coefficient + 1);
        }
    }

    public int State { get; set; }
}