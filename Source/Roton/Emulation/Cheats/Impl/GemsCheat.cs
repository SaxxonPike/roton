using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "GEMS" cheat, which increases the player's gem count.
/// </summary>
[Context(Context.Original, "GEMS")]
[Context(Context.Super, "GEMS")]
internal sealed class GemsCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear) => 
        world.Gems += 5;
}