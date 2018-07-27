using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "TORCHES")]
    public sealed class TorchesCheat : ICheat
    {
        private readonly IEngine _engine;

        public TorchesCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(string name, bool clear)
        {
            _engine.World.Torches += 3;
        }
    }
}