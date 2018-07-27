using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperFacts : Facts
    {
        public override int AmmoPerPickup => 20;
        public override int HealthPerGem => 10;
        public override string DefaultWorldName => "MONSTER";
        public override string ConfigFileName => null;
    }
}