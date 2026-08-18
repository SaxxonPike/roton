using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x12)]
[Context(Context.Super, 0x45)]
public sealed class BulletAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var canRicochet = true;
        while (true)
        {
            var target = actor.Location + actor.Vector;
            var element = Engine.Tiles.ElementAt(target);
            if (element.IsFloor || element.Id == Engine.ElementList.WaterId || element.Id == Engine.ElementList.LavaId)
            {
                Engine.MoveActor(index, target);
                break;
            }

            if (canRicochet && element.Id == Engine.ElementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector = -actor.Vector;
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            if (element.Id == Engine.ElementList.BreakableId ||
                element.IsDestructible && (element.Id == Engine.ElementList.PlayerId || actor.P1 == 0))
            {
                if (element.Points != 0)
                {
                    Engine.World.Score += element.Points;
                    Engine.UpdateStatus();
                }

                Engine.Attack(index, target);
                break;
            }

            if (canRicochet &&
                Engine.Tiles[actor.Location + actor.Vector.Clockwise()].Id == Engine.ElementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.CounterClockwise();
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            if (canRicochet &&
                Engine.Tiles[actor.Location + actor.Vector.CounterClockwise()].Id == Engine.ElementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.Clockwise();
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            Engine.RemoveActor(index);
            Engine.State.ActIndex--;
            if (element.Id == Engine.ElementList.ObjectId || element.Id == Engine.ElementList.ScrollId)
            {
                Engine.BroadcastLabel(-Engine.Actors.ActorIndexAt(target), Engine.Facts.ShotLabel, false);
            }

            break;
        }
    }
}