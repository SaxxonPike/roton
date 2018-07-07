using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class KeysCheat : ICheat
    {
        private readonly IWorld _world;

        public KeysCheat(IWorld world)
        {
            _world = world;
        }

        public string Name => "KEYS";
        
        public void Execute()
        {
            for (var i = 1; i < 8; i++)
                _world.Keys[i] = true;
        }
    }
}