using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "ALL" cheat, which when negated will clear all flags.
/// If not negated, it will set the "ALL" flag.
/// </summary>
[Context(Context.Super, "ALL")]
internal sealed class AllCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear)
    {
        if (clear)
            world.Flags.Clear();
        else
            world.Flags.Add("ALL");
    }
}