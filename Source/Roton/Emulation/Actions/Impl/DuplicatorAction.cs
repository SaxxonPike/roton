using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Interactions;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the duplicator element.
/// </summary>
[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
internal sealed class DuplicatorAction(
    IEngineAccessor engine,
    IInteractionList interactionList,
    IState state,
    IElementList elementList,
    ITiles tiles,
    IActorList actorList,
    ISounds sounds,
    ISoundUnit soundUnit,
    IBoardUpdater boardUpdater,
    IPusher pusher)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var source = actor.Location + actor.Vector;
        var target = actor.Location - actor.Vector;

        if (actor.P1 > 4)
        {
            if (tiles[target].Id == elementList.PlayerId)
            {
                interactionList.Get(tiles[source].Id)?
                    .Interact(source, 0, ref state.KeyVector);
            }
            else
            {
                if (tiles[target].Id != elementList.EmptyId)
                {
                    var oppVec = -actor.Vector;
                    pusher.Push(target, oppVec);
                }

                if (tiles[target].Id == elementList.EmptyId)
                {
                    var sourceIndex = actorList.ActorIndexAt(source);
                    if (sourceIndex > 0)
                    {
                        // This is a bug in the original code. Should be "- 2" instead of "+ 22".
                        // The call to SpawnActor won't actually spawn anything, but the update still happens.
                        // The bug is retained for compatibility.

                        if (state.ActorCount < actorList.Capacity + 22)
                        {
                            Engine.SpawnActor(target, tiles[source], actorList[sourceIndex].Cycle,
                                actorList[sourceIndex]);
                            boardUpdater.UpdateBoard(target);
                        }
                    }
                    else if (sourceIndex != 0)
                    {
                        tiles[target] = tiles[source];
                        boardUpdater.UpdateBoard(target);
                    }

                    soundUnit.PlaySound(3, sounds.Duplicate);
                }
                else
                {
                    soundUnit.PlaySound(3, sounds.DuplicateFail);
                }
            }

            actor.P1 = 0;
        }
        else
        {
            actor.P1++;
        }

        boardUpdater.UpdateBoard(actor.Location);
        actor.Cycle = (9 - actor.P2) * 3;
    }
}