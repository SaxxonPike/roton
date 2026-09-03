using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the spider element.
/// </summary>
[Context(Context.Super, 0x3E)]
internal sealed class SpiderAction(
    IActorList actors,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elements,
    IMover mover,
    INavigator navigator,
    IAttacker attacker)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];

        var vector = actor.P1 <= randomizer.GetNext(10)
            ? navigator.Rnd()
            : navigator.Seek(actor.Location);

        if (ActSpiderAttemptDirection(index, vector))
            return;

        var i = (randomizer.GetNext(2) << 1) - 1;

        if (ActSpiderAttemptDirection(index, (vector * i).Swap()))
            return;

        if (!ActSpiderAttemptDirection(index, -(vector * i).Swap()))
            ActSpiderAttemptDirection(index, -vector);
    }

    private bool ActSpiderAttemptDirection(int index, Vector vector)
    {
        var actor = actors[index];
        var target = actor.Location + vector;
        var targetElement = tiles.ElementAt(target).Id;

        if (targetElement == elements.WebId)
        {
            mover.MoveActor(index, target);
            return true;
        }

        if (targetElement == elements.PlayerId)
        {
            attacker.Attack(index, target);
            return true;
        }

        return false;
    }
}