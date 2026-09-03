using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "Z" cheat, which increases the number of stones possessed by the player.
/// </summary>
[Context(Context.Super, "Z")]
internal sealed class ZCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear) =>
        world.Stones++;
}