using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "TORCHES" cheat, which increases the player's torch count.
/// </summary>
/// <param name="world"></param>
[Context(Context.Original, "TORCHES")]
internal sealed class TorchesCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear) =>
        world.Torches += 3;
}