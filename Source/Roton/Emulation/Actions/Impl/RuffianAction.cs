using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the ruffian element.
/// </summary>
[Context(Context.Original, 0x23)]
[Context(Context.Super, 0x23)]
internal sealed class RuffianAction(
    IEngineAccessor engine,
    IActorList actors,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elements,
    IMover mover,
    INavigator navigator,
    IAttacker attacker)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];

        if (actor.Vector.IsZero())
        {
            if (actor.P2 + 8 <= randomizer.GetNext(17))
            {
                actor.Vector = actor.P1 >= randomizer.GetNext(9)
                    ? navigator.Seek(actor.Location)
                    : navigator.Rnd();
            }
        }
        else
        {
            if (actor.Location.X == actors.Player.Location.X || actor.Location.Y == actors.Player.Location.Y)
            {
                if (actor.P1 >= randomizer.GetNext(9))
                    actor.Vector = navigator.Seek(actor.Location);
            }

            var target = actor.Location + actor.Vector;

            if (tiles.ElementAt(target).Id == elements.PlayerId)
            {
                attacker.Attack(index, target);
            }
            else if (tiles.ElementAt(target).IsFloor)
            {
                mover.MoveActor(index, target);

                if (actor.P2 + 8 <= randomizer.GetNext(17))
                    actor.Vector = new Vector(0, 0);
            }
            else
            {
                actor.Vector = new Vector(0, 0);
            }
        }
    }
}