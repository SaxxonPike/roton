using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperFacts : Facts
{
    public override int AmmoPerPickup => 20;
    public override int HealthPerGem => 10;
    public override string DefaultWorldName => "MONSTER";
    public override int HighScoreNameLength => 60;
    public override EngineKeyCode StartGameKey => EngineKeyCode.Enter;
    public override string SavedGameExtension => "SAV";
    public override string WorldFileExtension => "SZT";
    public override string SavedGameWindowTitle => "Saved Games";
    public override string WorldFileWindowTitle => "Super ZZT Worlds";
    public override string HighScoreExtension => "HGS";
    public override int BaseMemoryUsage => 203044;
    public override int ScrollHeight => 23;
    public override int ScrollWidth => 37;
    public override int ScrollLeft => 1;
    public override int ScrollTop => 2;
}