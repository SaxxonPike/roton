using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "HEALTH")]
    [ContextEngine(ContextEngine.Super, "HEALTH")]
    public sealed class HealthCheat : ICheat
    {
        private readonly IEngine _engine;

        public HealthCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.World.Health += 50;
        }
    }
}