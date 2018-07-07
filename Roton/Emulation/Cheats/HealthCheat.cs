using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class HealthCheat : ICheat
    {
        private readonly IWorld _world;

        public HealthCheat(IWorld world)
        {
            _world = world;
        }
        
        public string Name => "HEALTH";
        
        public void Execute()
        {
            _world.Health += 50;
        }
    }
}