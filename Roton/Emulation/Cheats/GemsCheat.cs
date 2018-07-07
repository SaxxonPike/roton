using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class GemsCheat : ICheat
    {
        private readonly IWorld _world;

        public GemsCheat(IWorld world)
        {
            _world = world;
        }

        public string Name => "GEMS";
        
        public void Execute()
        {
            _world.Gems += 5;
        }
    }
}