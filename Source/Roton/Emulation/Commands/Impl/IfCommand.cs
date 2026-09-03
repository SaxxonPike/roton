using Roton.Emulation.Conditions;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "IF")]
[Context(Context.Super, "IF")]
internal sealed class IfCommand(
    IConditionEvaluator conditionEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (conditionEvaluator.TryEval(ref context, ref instruction, out var result))
            context.Resume = result;
    }
}