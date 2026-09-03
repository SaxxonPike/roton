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
    IInteractionList interactions,
    IState state,
    IElementList elements,
    ITiles tiles,
    IActorList actors,
    ISounds sounds,
    ISoundUnit soundUnit,
    IBoardUpdater boardUpdater,
    IPusher pusher,
    ISpawner spawner)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];
        var source = actor.Location + actor.Vector;
        var target = actor.Location - actor.Vector;

        if (actor.P1 > 4)
        {
            if (tiles[target].Id == elements.PlayerId)
            {
                interactions.Get(tiles[source].Id)?
                    .Interact(source, 0, ref state.KeyVector);
            }
            else
            {
                if (tiles[target].Id != elements.EmptyId)
                {
                    var oppVec = -actor.Vector;
                    pusher.Push(target, oppVec);
                }

                if (tiles[target].Id == elements.EmptyId)
                {
                    var sourceIndex = actors.ActorIndexAt(source);
                    if (sourceIndex > 0)
                    {
                        // This is a bug in the original code. Should be "- 2" instead of "+ 22".
                        // The call to SpawnActor won't actually spawn anything, but the update still happens.
                        // The bug is retained for compatibility.

                        if (state.ActorCount < actors.Capacity + 22)
                        {
                            spawner.SpawnActor(target, tiles[source], actors[sourceIndex].Cycle,
                                actors[sourceIndex]);
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