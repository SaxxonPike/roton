using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x3B)]
[Context(Context.Super, 0x3B)]
internal sealed class RotonAction(
    IEngineAccessor engine,
    IActorList actorList,
    IRandomizer randomizer,
    IElementList elementList,
    ITiles tiles)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];

        actor.P3--;
        if (actor.P3 < -actor.P2 % 10)
        {
            actor.P3 = unchecked((byte)(actor.P2 * 10 + randomizer.GetNext(10)));
        }

        actor.Vector = Engine.Seek(actor.Location);
        if (actor.P1 <= randomizer.GetNext(10))
        {
            var temp = actor.Vector.X;
            actor.Vector.X = -((int)actor.P2).Polarity() * actor.Vector.Y;
            actor.Vector.Y = ((int)actor.P2).Polarity() * temp;
        }

        var target = actor.Location + actor.Vector;
        if (tiles.ElementAt(target).IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (tiles[target].Id == elementList.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}