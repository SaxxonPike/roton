using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Randomizer : IRandomizer
    {
        private readonly Lazy<IRandomState> _randomState;
        private IRandomState RandomState => _randomState.Value;

        public Randomizer(Lazy<IConfig> config)
        {
            _randomState = new Lazy<IRandomState>(() => 
                config.Value.RandomSeed.HasValue ? 
                new RandomState(config.Value.RandomSeed.Value) :
                new RandomState());
        }

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
}