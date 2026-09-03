using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "NOT")]
[Context(Context.Super, "NOT")]
internal sealed class NotCondition(
    IConditionEvaluator conditionEvaluator)
    : ICondition
{
    public bool? Execute(ref OopContext context, ref Word instruction) =>
        conditionEvaluator.TryEval(ref context, ref instruction, out var result)
            ? !result
            : null;
}