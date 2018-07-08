using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class HealthCheat : ICheat
    {
        private readonly IEngine _engine;

        public HealthCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "HEALTH";
        
        public void Execute()
        {
            _engine.World.Health += 50;
        }
    }
}