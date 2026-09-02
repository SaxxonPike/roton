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
    private RandomState _randomState;

    private int GetSeed()
    {
        if (config.RandomSeed is { } seed)
            return seed;

        var time = DateTimeOffset.Now;
        seed = (time.Second << 24) |
               ((time.Millisecond / 10) << 16) |
               (time.Hour << 8) |
               time.Minute;

        return seed;
    }

    public void Initialize()
    {
        var seed = GetSeed();
        _randomState.Seed = seed;
        _randomState.State = seed;
    }

    public int GetNext(int exclusiveUpperBound)
    {
        unchecked
        {
            var newState = _randomState.State * 33797 + 1;
            _randomState.State = newState;
        }

        if (exclusiveUpperBound == 0)
            return 0;

        return ((_randomState.State >> 16) & 0xFFFF) % exclusiveUpperBound;
    }

    public int Seed
    {
        get => _randomState.Seed;
        set => _randomState.Seed = value;
    }

    public int State
    {
        get => _randomState.State;
        set => _randomState.State = value;
    }
}