using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "AMMO" cheat, which increases the player's ammo count.
/// </summary>
[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
internal sealed class AmmoCheat(
    IFacts facts,
    IWorld world)
    : ICheat
{
    private IFacts Facts => facts;

    public void Execute(bool clear) => 
        world.Ammo += Facts.AmmoPerPickup;
}