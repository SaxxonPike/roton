using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
internal sealed class AmmoCheat(
    IFacts facts,
    IWorld world)
    : ICheat
{
    private IFacts Facts => facts;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        world.Ammo += Facts.AmmoPerPickup;
    }
}