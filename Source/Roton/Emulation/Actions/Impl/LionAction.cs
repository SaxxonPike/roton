using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x29)]
[Context(Context.Super, 0x29)]
public sealed class LionAction(Lazy<IEngine> engine) : IAction
{
    private IEngine Engine => engine.Value;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var vector = new Vector();

        vector.CopyFrom(actor.P1 >= Engine.Random.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd());

        var target = actor.Location.Sum(vector);
        var element = Engine.Tiles.ElementAt(target);
        if (element.IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (element.Id == Engine.ElementList.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}