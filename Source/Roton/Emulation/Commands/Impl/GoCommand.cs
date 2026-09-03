using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "GO")]
[Context(Context.Super, "GO")]
internal sealed class GoCommand(
    ITiles tiles,
    IPusher pusher,
    IMover mover,
    IDirectionEvaluator directionEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!directionEvaluator.TryEval(ref context, ref instruction, out var vec))
            return;

        var target = context.Actor.Location + vec;

        if (!tiles.ElementAt(target).IsFloor)
            pusher.Push(target, vec);

        if (tiles.ElementAt(target).IsFloor)
        {
            mover.MoveActor(context.Index, target);
            context.Moved = true;
        }
        else
        {
            context.Repeat = true;
        }
    }
}