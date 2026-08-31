using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "BLOCKED")]
[Context(Context.Super, "BLOCKED")]
internal sealed class BlockedCondition(
    IParser parser,
    ITiles tiles)
    : ICondition
{
    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        if (!parser.TryEvalDirection(ref context, ref instruction, out var val))
            return null;

        return !tiles.ElementAt(context.Actor.Location + val).IsFloor;
    }
}