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

        if (actor.Vector.IsZero())
        {
            if (actor.P2 + 8 <= randomizer.GetNext(17))
            {
                actor.Vector = actor.P1 >= randomizer.GetNext(9)
                    ? Engine.Seek(actor.Location)
                    : Engine.Rnd();
            }
        }
        else
        {
            if (actor.Location.X == actorList.Player.Location.X || actor.Location.Y == actorList.Player.Location.Y)
            {
                if (actor.P1 >= randomizer.GetNext(9))
                    actor.Vector = Engine.Seek(actor.Location);
            }

            var target = actor.Location + actor.Vector;

            if (tiles.ElementAt(target).Id == elementList.PlayerId)
            {
                Engine.Attack(index, target);
            }
            else if (Engine.ElementAt(target).IsFloor)
            {
                Engine.MoveActor(index, target);

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