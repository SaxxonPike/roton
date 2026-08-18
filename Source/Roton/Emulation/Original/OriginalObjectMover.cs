using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalObjectMover(IEngineAccessor engine) : IObjectMover
{
    private IEngine Engine => engine.Instance;

    public void ExecuteDirection(IOopContext context, Vector vector)
    {
        if (vector.IsZero())
        {
            context.Repeat = false;
        }
        else
        {
            var target = context.Actor.Location + vector;
            if (!Engine.ElementAt(target).IsFloor) 
                Engine.Push(target, vector);

            if (Engine.ElementAt(target).IsFloor)
            {
                Engine.MoveActor(context.Index, target);
                context.Repeat = false;
            }
        }
    }
}