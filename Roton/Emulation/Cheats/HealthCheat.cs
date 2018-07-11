using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt, "HEALTH")]
    [ContextEngine(ContextEngine.SuperZzt, "HEALTH")]
    public sealed class HealthCheat : ICheat
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