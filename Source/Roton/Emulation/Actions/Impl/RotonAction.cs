using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x3B)]
[Context(Context.Super, 0x3B)]
public sealed class RotonAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public RotonAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Act(int index)
    {
        var actor = Engine.Actors[index];

        actor.P3--;
        if (actor.P3 < -actor.P2 % 10)
        {
            actor.P3 = actor.P2 * 10 + Engine.Random.GetNext(10);
        }

        actor.Vector.CopyFrom(Engine.Seek(actor.Location));
        if (actor.P1 <= Engine.Random.GetNext(10))
        {
            var temp = actor.Vector.X;
            actor.Vector.X = -actor.P2.Polarity() * actor.Vector.Y;
            actor.Vector.Y = actor.P2.Polarity() * temp;
        }

        var target = actor.Location.Sum(actor.Vector);
        if (Engine.Tiles.ElementAt(target).IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (Engine.Tiles[target].Id == Engine.ElementList.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}