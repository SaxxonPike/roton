using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoCheat(IEngineAccessor engine, IFacts facts) : ICheat
{
    private IEngine Engine => engine.Instance;
    private IFacts Facts => facts;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.World.Ammo += Facts.AmmoPerPickup;
    }
}