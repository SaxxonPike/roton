using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalObjectMover(
    IEngineAccessor engine,
    IPusher pusher) : IObjectMover
{
    private IEngine Engine => engine.Instance;

    public void ExecuteDirection(ref OopContext context, Vector vector)
    {
        if (vector.IsZero())
        {
            context.Repeat = false;
        }
        else
        {
            var target = context.Actor.Location + vector;

            if (!Engine.ElementAt(target).IsFloor)
                pusher.Push(target, vector);

            if (!Engine.ElementAt(target).IsFloor)
                return;

            Engine.MoveActor(context.Index, target);
            context.Repeat = false;
        }
    }
}