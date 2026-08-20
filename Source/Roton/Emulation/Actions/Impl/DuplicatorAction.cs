using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
public sealed class DuplicatorAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var source = actor.Location + actor.Vector;
        var target = actor.Location - actor.Vector;

        if (actor.P1 > 4)
        {
            if (Engine.Tiles[target].Id == Engine.Elements.PlayerId)
            {
                Engine.InteractionList.Get(Engine.Tiles[source].Id)
                    .Interact(source, 0, ref Engine.State.KeyVector);
            }
            else
            {
                if (Engine.Tiles[target].Id != Engine.Elements.EmptyId)
                {
                    var oppVec = -actor.Vector;
                    Engine.Push(target, oppVec);
                }

                if (Engine.Tiles[target].Id == Engine.Elements.EmptyId)
                {
                    var sourceIndex = Engine.Actors.ActorIndexAt(source);
                    if (sourceIndex > 0)
                    {
                        if (Engine.State.ActorCount < Engine.Actors.Capacity - 2)
                        {
                            ref var sourceTile = ref Engine.Tiles[source];
                            Engine.SpawnActor(target, sourceTile, Engine.Actors[sourceIndex].Cycle,
                                Engine.Actors[sourceIndex]);
                            Engine.UpdateBoard(target);
                        }
                    }
                    else if (sourceIndex != 0)
                    {
                        Engine.Tiles[target] = Engine.Tiles[source];
                        Engine.UpdateBoard(target);
                    }

                    Engine.PlaySound(3, Engine.Sounds.Duplicate);
                }
                else
                {
                    Engine.PlaySound(3, Engine.Sounds.DuplicateFail);
                }
            }

            actor.P1 = 0;
        }
        else
        {
            actor.P1++;
        }

        Engine.UpdateBoard(actor.Location);
        actor.Cycle = (9 - actor.P2) * 3;
    }
}