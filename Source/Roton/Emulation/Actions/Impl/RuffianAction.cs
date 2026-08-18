using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x23)]
[Context(Context.Super, 0x23)]
public sealed class RuffianAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];

        if (actor.Vector.IsZero())
        {
            if (actor.P2 + 8 <= Engine.Random.GetNext(17))
            {
                actor.Vector = actor.P1 >= Engine.Random.GetNext(9)
                    ? Engine.Seek(actor.Location)
                    :Engine.Rnd();
                
            }
        }
        else
        {
            if (actor.Location.X == Engine.Player.Location.X || actor.Location.Y == Engine.Player.Location.Y)
            {
                if (actor.P1 >= Engine.Random.GetNext(9))
                {
                    actor.Vector = Engine.Seek(actor.Location);
                }
            }

            var target = actor.Location + actor.Vector;
            if (Engine.Tiles.ElementAt(target).Id == Engine.ElementList.PlayerId)
            {
                Engine.Attack(index, target);
            }
            else if (Engine.ElementAt(target).IsFloor)
            {
                Engine.MoveActor(index, target);
                if (actor.P2 + 8 <= Engine.Random.GetNext(17))
                {
                    actor.Vector = new Vector(0, 0);
                }
            }
            else
            {
                actor.Vector = new Vector(0, 0);
            }
        }
    }
}