using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x22)]
[Context(Context.Super, 0x22)]
public sealed class BearAction(
    IEngineAccessor engine,
    IActorList actorList,
    IElementList elementList,
    ITiles tiles) 
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var vector = new Vector();

        if (actorList.Player.Location.X == actor.Location.X ||
            8 - actor.P1 < actorList.Player.Location.Y.AbsDiff(actor.Location.Y))
        {
            vector = new Vector(0,
                8 - actor.P1 < actorList.Player.Location.X.AbsDiff(actor.Location.X)
                    ? 0
                    : (actorList.Player.Location.Y - actor.Location.Y).Polarity());
        }
        else
        {
            vector = new Vector((actorList.Player.Location.X - actor.Location.X).Polarity(), 0);
        }

        var target = actor.Location + vector;
        var targetElement = tiles.ElementAt(target);

        if (targetElement.IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (targetElement.Id == elementList.PlayerId || targetElement.Id == elementList.BreakableId)
        {
            Engine.Attack(index, target);
        }
    }
}