using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalObjectMover(
    IPusher pusher,
    IMover mover,
    ITiles tiles)
    : IObjectMover
{
    public void ExecuteDirection(ref OopContext context, Vector vector)
    {
        if (vector.IsZero())
        {
            context.Repeat = false;
            return;
        }

        var target = context.Actor.Location + vector;

        if (!tiles.ElementAt(target).IsFloor)
            pusher.Push(target, vector);

        if (!tiles.ElementAt(target).IsFloor)
            return;

        mover.Move(context.Index, target);
        context.Repeat = false;
    }
}