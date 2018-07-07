using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class TorchesCheat : ICheat
    {
        private readonly IWorld _world;

        public TorchesCheat(IWorld world)
        {
            _world = world;
        }
        
        public string Name => "TORCHES";
        
        public void Execute()
        {
            _world.Torches += 3;
        }
    }
}