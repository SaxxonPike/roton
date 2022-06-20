using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x26)]
public sealed class SharkAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public SharkAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var vector = new Vector();

        vector.CopyFrom(actor.P1 > Engine.Random.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd());

        var target = actor.Location.Sum(vector);
        var targetElement = Engine.Tiles.ElementAt(target);

        if (targetElement.Id == Engine.ElementList.WaterId || targetElement.Id == Engine.ElementList.LavaId)
        {
            Engine.MoveActor(index, target);
        }
        else if (targetElement.Id == Engine.ElementList.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}