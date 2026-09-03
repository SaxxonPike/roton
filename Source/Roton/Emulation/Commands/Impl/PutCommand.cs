using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PUT")]
[Context(Context.Super, "PUT")]
internal sealed class PutCommand(
    IErrorRaiser errorRaiser,
    IPlotter plotter,
    IDirectionEvaluator directionEvaluator,
    IKindEvaluator kindEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;

        if (directionEvaluator.TryEval(ref context, ref instruction, out var vec))
        {
            if (kindEvaluator.TryEval(ref context, ref instruction, out var k))
            {
                success = true;

                var target = context.Actor.Location + vec;
                plotter.Put(target, vec, k);
            }
        }

        if (!success)
            errorRaiser.RaiseError(ref context, "Bad #PUT");
    }
}