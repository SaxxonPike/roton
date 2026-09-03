using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "WALK")]
[Context(Context.Super, "WALK")]
internal sealed class WalkCommand(
    IDirectionEvaluator directionEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (directionEvaluator.TryEval(ref context, ref instruction, out var vec)) 
            context.Actor.Vector = vec;
    }
}