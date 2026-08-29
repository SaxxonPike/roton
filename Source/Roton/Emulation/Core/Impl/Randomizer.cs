using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Randomizer(IConfig config) : IRandomizer
{
    private RandomState RandomState { get; } = 
        config.RandomSeed.HasValue
            ? new RandomState(config.RandomSeed.Value) 
            : new RandomState();

    public int GetNext(int exclusiveUpperBound)
    {
        unchecked
        {
            var newState = RandomState.State * 33797 + 1;
            RandomState.State = newState;
        }

        if (exclusiveUpperBound == 0)
            return 0;

        return ((RandomState.State >> 16) & 0xFFFF) % exclusiveUpperBound;
    }
}