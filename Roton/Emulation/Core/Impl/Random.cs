using Roton.Emulation.Execution;

namespace Roton.Core
{
    public class Random : IRandom
    {
        private readonly IRandomizer _random;
        private readonly IRandomizer _syncRandom;

        public Random(IConfig config)
        {
            _random = new Randomizer(new RandomState());
            _syncRandom = new Randomizer(new RandomState(config.RandomSeed));
        }

        public int NonSynced(int exclusiveMax)
        {
            return _random.GetNext(exclusiveMax);
        }

        public int Synced(int exclusiveMax)
        {
            return _syncRandom.GetNext(exclusiveMax);
        }
    }
}