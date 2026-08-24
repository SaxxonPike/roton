using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ENERGIZED")]
[Context(Context.Super, "ENERGIZED")]
public sealed class EnergizedCondition(
    IWorld world)
    : ICondition
{
    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        return world.EnergyCycles > 0;
    }
}