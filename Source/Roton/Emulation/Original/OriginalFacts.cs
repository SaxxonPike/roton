using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalFacts : Facts
{
    public override int AmmoPerPickup => 5;
    public override int HealthPerGem => 1;
    public override string DefaultWorldName => "TOWN";
    public override int HighScoreNameLength => 50;
    public override EngineKeyCode StartGameKey => EngineKeyCode.P;
    public override string SavedGameExtension => "SAV";
    public override string WorldFileExtension => "ZZT";
    public override string SavedGameWindowTitle => "Saved Games";
    public override string WorldFileWindowTitle => "ZZT Worlds";
    public override int TorchRadius => 50;
    public override int DistanceMultY => 2;
    public override int RadiusBoundX => 9;
    public override int RadiusBoundY => 6;
    public override string HighScoreExtension => "HI";
    public override int BaseMemoryUsage => 205791;
    public override int ScrollHeight => 19;
    public override int ScrollWidth => 49;
    public override int ScrollLeft => 5;
    public override int ScrollTop => 3;
}