using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalFacts : Facts
    {
        public override int AmmoPerPickup => 5;
        public override int HealthPerGem => 1;
        public override string DefaultWorldName => "TOWN";
        public override string ConfigFileName => "ZZT.CFG";
    }
}