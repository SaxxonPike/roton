using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super, 0x24)]
internal sealed class SuperObjectAction(
    IEngineAccessor engine,
    IActorList actorList,
    ITiles tiles,
    IFacts facts,
    IBroadcaster broadcaster,
    IPusher pusher,
    IMover mover)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        if (actor.P2 == 0 && actor.Instruction >= 0)
        {
            Engine.ExecuteCode(index, ref actor.Instruction, "Interaction");
        }

        if (actor.Vector.IsZero())
        {
            if (actor.P2 > 0)
                actor.P2--;
            return;
        }

        var target = actor.Location + actor.Vector;

        if (!tiles.ElementAt(target).IsFloor)
            pusher.Push(target, actor.Vector);

        if (tiles.ElementAt(target).IsFloor)
        {
            mover.MoveActor(index, target);

            if (actor.P2 <= 0)
                return;

            actor.P2--;
            if (actor.P2 == 0)
                actor.Vector = Vector.Idle;
        }
        else
        {
            broadcaster.BroadcastLabel(-index, facts.ThudLabel, false);
        }
    }
}