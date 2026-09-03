using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the Roton element.
/// </summary>
[Context(Context.Original, 0x3B)]
[Context(Context.Super, 0x3B)]
internal sealed class RotonAction(
    IActorList actors,
    IRandomizer randomizer,
    IElementList elements,
    ITiles tiles,
    IMover mover,
    INavigator navigator,
    IAttacker attacker)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];

        actor.P3--;

        if (actor.P3 < -actor.P2 % 10)
            actor.P3 = actor.P2 * 10 + randomizer.GetNext(10);

        actor.Vector = navigator.Seek(actor.Location);

        if (actor.P1 <= randomizer.GetNext(10))
        {
            var temp = actor.Vector.X;
            actor.Vector.X = -((int)actor.P2).Polarity() * actor.Vector.Y;
            actor.Vector.Y = ((int)actor.P2).Polarity() * temp;
        }

        var target = actor.Location + actor.Vector;

        if (tiles.ElementAt(target).IsFloor)
            mover.MoveActor(index, target);
        else if (tiles[target].Id == elements.PlayerId)
            attacker.Attack(index, target);
    }
}