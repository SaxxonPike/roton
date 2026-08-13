using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalObjectMover : IObjectMover
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public OriginalObjectMover(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void ExecuteDirection(IOopContext context, IXyPair vector)
    {
        if (vector.IsZero())
        {
            context.Repeat = false;
        }
        else
        {
            var target = context.Actor.Location.Sum(vector);
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