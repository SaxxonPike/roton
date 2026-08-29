using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x3E)]
internal sealed class SpiderAction(
    IEngineAccessor engine,
    IActorList actorList,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elementList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var vector = new Vector();

        vector = actor.P1 <= randomizer.GetNext(10)
            ? Engine.Rnd()
            : Engine.Seek(actor.Location);

        if (!ActSpiderAttemptDirection(index, vector))
        {
            var i = (randomizer.GetNext(2) << 1) - 1;
            if (!ActSpiderAttemptDirection(index, (vector * i).Swap()))
            {
                if (!ActSpiderAttemptDirection(index, -(vector * i).Swap()))
                {
                    ActSpiderAttemptDirection(index, -vector);
                }
            }
        }
    }

    private bool ActSpiderAttemptDirection(int index, Vector vector)
    {
        var actor = actorList[index];
        var target = actor.Location + vector;
        var targetElement = tiles.ElementAt(target).Id;

        if (targetElement == elementList.WebId)
        {
            Engine.MoveActor(index, target);
            return true;
        }

        if (targetElement == elementList.PlayerId)
        {
            Engine.Attack(index, target);
            return true;
        }

        return false;
    }
}