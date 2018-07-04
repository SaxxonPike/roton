using Roton.Emulation.Execution;

namespace Roton.Core
{
    public interface IConfig
    {
        int RandomSeed { get; }
        int AmmoPerPickup { get; }
    }
    
    public class RandomizerService : IRandomizerService
    {
        private readonly IRandomizer _random;
        private readonly IRandomizer _syncRandom;

        public RandomizerService(IConfig config)
        {
            _random = new Randomizer(new RandomState());
            _syncRandom = new Randomizer(new RandomState(config.RandomSeed));
        }

        public int Random(int exclusiveMax)
        {
            return _random.GetNext(exclusiveMax);
        }

        public int SyncRandom(int exclusiveMax)
        {
            return _syncRandom.GetNext(exclusiveMax);
        }
    }
}