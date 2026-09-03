using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the pusher element.
/// </summary>
[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
internal sealed class PusherAction(
    IActorList actors,
    ITiles tiles,
    ISounds sounds,
    IElementList elements,
    IActionList actions,
    ISoundPlayer soundPlayer,
    IPusher pusher,
    IMover mover)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];
        var source = actor.Location;

        if (!tiles.ElementAt(actor.Location + actor.Vector).IsFloor)
            pusher.Push(actor.Location + actor.Vector, actor.Vector);

        index = actors.ActorIndexAt(source);
        actor = actors[index];

        if (!tiles.ElementAt(actor.Location + actor.Vector).IsFloor)
            return;

        var behindLocation = actor.Location - actor.Vector;
        mover.MoveActor(index, actor.Location + actor.Vector);
        soundPlayer.PlaySound(2, sounds.Push);

        if (tiles[behindLocation].Id != elements.PusherId)
            return;

        var behindIndex = actors.ActorIndexAt(behindLocation);
        var behindActor = actors[behindIndex];

        if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            actions.Get(elements.PusherId)?.Act(behindIndex);
    }
}