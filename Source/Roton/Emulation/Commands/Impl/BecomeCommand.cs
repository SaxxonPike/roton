using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BECOME")]
[Context(Context.Super, "BECOME")]
internal sealed class BecomeCommand(
    IErrorRaiser errorRaiser,
    IKindEvaluator kindEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!kindEvaluator.TryEval(ref context, ref instruction, out var val))
        {
            errorRaiser.RaiseError(ref context, "Bad #BECOME");
            return;
        }

        context.Died = true;
        context.DeathTile = val;
    }
}