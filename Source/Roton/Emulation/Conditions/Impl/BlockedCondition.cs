using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "BLOCKED")]
[Context(Context.Super, "BLOCKED")]
public sealed class BlockedCondition(IEngineAccessor engine) : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        var direction = Engine.Parser.GetDirection(ref context, ref instruction);
        if (direction is not {} val)
            return null;

        return !Engine.ElementAt(context.Actor.Location + val).IsFloor;
    }
}