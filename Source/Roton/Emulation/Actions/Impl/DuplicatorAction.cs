using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
public sealed class DuplicatorAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public DuplicatorAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var source = actor.Location.Sum(actor.Vector);
        var target = actor.Location.Difference(actor.Vector);

        if (actor.P1 > 4)
        {
            if (Engine.Tiles[target].Id == Engine.ElementList.PlayerId)
            {
                Engine.InteractionList.Get(Engine.Tiles[source].Id)
                    .Interact(source, 0, Engine.State.KeyVector);
            }
            else
            {
                if (Engine.Tiles[target].Id != Engine.ElementList.EmptyId)
                {
                    Engine.Push(target, actor.Vector.Opposite());
                }

                if (Engine.Tiles[target].Id == Engine.ElementList.EmptyId)
                {
                    var sourceIndex = Engine.Actors.ActorIndexAt(source);
                    if (sourceIndex > 0)
                    {
                        if (Engine.State.ActorCount < Engine.Actors.Capacity - 2)
                        {
                            var sourceTile = Engine.Tiles[source];
                            Engine.SpawnActor(target, sourceTile, Engine.ElementList[sourceTile.Id].Cycle,
                                Engine.Actors[sourceIndex]);
                            Engine.UpdateBoard(target);
                        }
                    }
                    else if (sourceIndex != 0)
                    {
                        Engine.Tiles[target].CopyFrom(Engine.Tiles[source]);
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