using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ANY")]
[Context(Context.Super, "ANY")]
internal sealed class AnyCondition(
    ITileFinder tileFinder,
    IKindEvaluator kindEvaluator) 
    : ICondition
{
    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        if (!kindEvaluator.TryEval(ref context, ref instruction, out var val))
            return null;

        return tileFinder.Find(val, new Location(0, 1));
    }
}