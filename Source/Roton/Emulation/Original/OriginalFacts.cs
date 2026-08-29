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
    public override string HighScoreExtension => "HI";
}