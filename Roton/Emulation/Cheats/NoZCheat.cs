using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Super, "NOZ")]
    public sealed class NoZCheat : ICheat
    {
        private readonly IEngine _engine;

        public NoZCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.World.Stones--;
        }
    }
}