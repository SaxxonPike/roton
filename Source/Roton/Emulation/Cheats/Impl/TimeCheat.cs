using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "TIME" cheat, which grants the player extra time on the board.
/// </summary>
[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
internal sealed class TimeCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear) => 
        world.TimePassed -= 30;
}