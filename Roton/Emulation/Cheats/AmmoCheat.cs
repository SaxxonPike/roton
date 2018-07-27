using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "AMMO")]
    [ContextEngine(ContextEngine.Super, "AMMO")]
    public sealed class AmmoCheat : ICheat
    {
        private readonly IEngine _engine;
        private readonly IFacts _facts;

        public AmmoCheat(IEngine engine, IFacts facts)
        {
            _engine = engine;
            _facts = facts;
        }

        public void Execute(string name, bool clear)
        {
            _engine.World.Ammo += _facts.AmmoPerPickup;
        }
    }
}