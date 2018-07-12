using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperFacts : Facts
    {
        public override int AmmoPerPickup => 20;
        public override int HealthPerGem => 10;
        public override string DefaultWorldName => "MONSTER";
        public override string ConfigFileName => null;
    }
}