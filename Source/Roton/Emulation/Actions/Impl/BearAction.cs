using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x22)]
[Context(Context.Super, 0x22)]
public sealed class BearAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var vector = new Vector();

            if (Engine.Player.Location.X == actor.Location.X ||
                8 - actor.P1 < Engine.Player.Location.Y.AbsDiff(actor.Location.Y))
            {
                vector = new Vector(0,
                    8 - actor.P1 < Engine.Player.Location.X.AbsDiff(actor.Location.X)
                        ? 0
                        : (Engine.Player.Location.Y - actor.Location.Y).Polarity());
            }
            else
            {
                vector = new Vector((Engine.Player.Location.X - actor.Location.X).Polarity(), 0);
            }

        var target = actor.Location + vector;
        var targetElement = Engine.Tiles.ElementAt(target);

        if (targetElement.IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (targetElement.Id == Engine.ElementList.PlayerId || targetElement.Id == Engine.ElementList.BreakableId)
        {
            Engine.Attack(index, target);
        }
    }
}