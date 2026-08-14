using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x3E)]
public sealed class SpiderAction(Lazy<IEngine> engine) : IAction
{
    private IEngine Engine => engine.Value;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var vector = new Vector();

        vector.CopyFrom(actor.P1 <= Engine.Random.GetNext(10)
            ? Engine.Rnd()
            : Engine.Seek(actor.Location));

        if (!ActSpiderAttemptDirection(index, vector))
        {
            var i = (Engine.Random.GetNext(2) << 1) - 1;
            if (!ActSpiderAttemptDirection(index, vector.Product(i).Swap()))
            {
                if (!ActSpiderAttemptDirection(index, vector.Product(i).Swap().Opposite()))
                {
                    ActSpiderAttemptDirection(index, vector.Opposite());
                }
            }
        }
    }

    private bool ActSpiderAttemptDirection(int index, IXyPair vector)
    {
        var actor = Engine.Actors[index];
        var target = actor.Location.Sum(vector);
        var targetElement = Engine.Tiles.ElementAt(target).Id;

        if (targetElement == Engine.ElementList.WebId)
        {
            Engine.MoveActor(index, target);
            return true;
        }

        if (targetElement == Engine.ElementList.PlayerId)
        {
            Engine.Attack(index, target);
            return true;
        }

        return false;
    }
}