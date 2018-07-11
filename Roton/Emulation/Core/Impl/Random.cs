using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class Random : IRandom
    {
        private readonly IRandomizer _random;
        private readonly IRandomizer _syncRandom;

        public Random(IConfig config)
        {
            _random = new Randomizer(new RandomState());
            _syncRandom = new Randomizer(new RandomState(config.RandomSeed));
        }

        public int NonSynced(int exclusiveMax) 
            => _random.GetNext(exclusiveMax);

        public int Synced(int exclusiveMax) 
            => _syncRandom.GetNext(exclusiveMax);
    }
}