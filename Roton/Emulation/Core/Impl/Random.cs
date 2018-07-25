using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Random : IRandom
    {
        private readonly IRandomizer _random;
        private readonly IRandomizer _syncRandom;

        public Random(IConfig config)
        {
            _random = new Randomizer(new RandomState());
            _syncRandom = new Randomizer(config.RandomSeed.HasValue 
                ? new RandomState(config.RandomSeed.Value) 
                : new RandomState());
        }

        public int NonSynced(int exclusiveMax) 
            => _random.GetNext(exclusiveMax);

        public int Synced(int exclusiveMax) 
            => _syncRandom.GetNext(exclusiveMax);
    }
}