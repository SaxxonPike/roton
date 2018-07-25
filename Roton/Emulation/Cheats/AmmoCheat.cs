using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "AMMO")]
    [ContextEngine(ContextEngine.Super, "AMMO")]
    public sealed class AmmoCheat : ICheat
    {
        private readonly IEngine _engine;

        public AmmoCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.World.Ammo += 5;
        }
    }
}