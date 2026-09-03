using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "NOZ" cheat, which clears the stone count.
/// </summary>
[Context(Context.Super, "NOZ")]
internal sealed class NoZCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear) =>
        world.Stones = -1;
}