using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
public sealed class PusherAction(
    IEngineAccessor engine,
    IActorList actorList,
    ITiles tiles,
    ISounds sounds,
    IElementList elementList,
    IActionList actionList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var source = actor.Location;

        if (!tiles.ElementAt(actor.Location + actor.Vector).IsFloor)
        {
            Engine.Push(actor.Location + actor.Vector, actor.Vector);
        }

        index = actorList.ActorIndexAt(source);
        actor = actorList[index];

        if (!tiles.ElementAt(actor.Location + actor.Vector).IsFloor)
            return;

        var behindLocation = actor.Location - actor.Vector;
        Engine.MoveActor(index, actor.Location + actor.Vector);
        Engine.PlaySound(2, sounds.Push);

        if (tiles[behindLocation].Id != elementList.PusherId)
            return;

        var behindIndex = actorList.ActorIndexAt(behindLocation);
        var behindActor = actorList[behindIndex];
        if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
        {
            actionList.Get(elementList.PusherId)?.Act(behindIndex);
        }
    }
}