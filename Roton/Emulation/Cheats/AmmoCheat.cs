using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class AmmoCheat : ICheat
    {
        private readonly IEngine _engine;

        public AmmoCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "AMMO";
        
        public void Execute()
        {
            _engine.World.Ammo += 5;
        }
    }
}