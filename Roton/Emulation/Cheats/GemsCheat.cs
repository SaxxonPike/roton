using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "GEMS")]
    [ContextEngine(ContextEngine.Super, "GEMS")]
    public sealed class GemsCheat : ICheat
    {
        private readonly IEngine _engine;

        public GemsCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.World.Gems += 5;
        }
    }
}