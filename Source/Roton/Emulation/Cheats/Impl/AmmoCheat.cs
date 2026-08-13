using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoCheat : ICheat
{
    private readonly Lazy<IEngine> _engine;
    private readonly Lazy<IFacts> _facts;

    public AmmoCheat(Lazy<IEngine> engine, Lazy<IFacts> facts)
    {
        _engine = engine;
        _facts = facts;
    }

    private IEngine Engine => _engine.Value;
    private IFacts Facts => _facts.Value;

    public void Execute(string name, bool clear)
    {
        Engine.World.Ammo += Facts.AmmoPerPickup;
    }
}