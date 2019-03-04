using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalFacts : Facts
    {
        public override int AmmoPerPickup => 5;
        public override int HealthPerGem => 1;
        public override string DefaultWorldName => "TOWN";
        public override int HighScoreNameLength => 50;
    }
}