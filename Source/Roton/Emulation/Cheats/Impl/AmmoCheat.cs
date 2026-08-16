using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoCheat(Lazy<IEngine> engine, Lazy<IFacts> facts) : ICheat
{
    private IEngine Engine => engine.Value;
    private IFacts Facts => facts.Value;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.World.Ammo += Facts.AmmoPerPickup;
    }
}