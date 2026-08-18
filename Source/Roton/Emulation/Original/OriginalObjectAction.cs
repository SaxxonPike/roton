using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original, 0x24)]
public sealed class OriginalObjectAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        if (actor.Instruction >= 0)
        {
            Engine.ExecuteCode(index, actor, @"Interaction");
        }
            
        if (actor.Vector.IsZero()) 
            return;

        var target = actor.Location + actor.Vector;
        if (Engine.Tiles.ElementAt(target).IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else
        {
            Engine.BroadcastLabel(-index, Engine.Facts.ThudLabel, false);
        }
    }
}