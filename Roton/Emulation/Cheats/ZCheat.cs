using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Super, "Z")]
    public sealed class ZCheat : ICheat
    {
        private readonly IEngine _engine;

        public ZCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.World.Stones++;
        }
    }
}