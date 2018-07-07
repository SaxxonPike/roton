using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class AmmoCheat : ICheat
    {
        private readonly IWorld _world;

        public AmmoCheat(IWorld world)
        {
            _world = world;
        }
        
        public string Name => "AMMO";
        
        public void Execute()
        {
            _world.Ammo += 5;
        }
    }
}