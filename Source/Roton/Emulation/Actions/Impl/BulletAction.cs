using Roton.Emulation.Core;
using Roton.Infrastructure;

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
            var element = tiles.ElementAt(target);
            if (element.IsFloor || element.Id == Engine.Elements.WaterId || element.Id == Engine.Elements.LavaId)
            {
                Engine.MoveActor(index, target);
                break;
            }

            if (canRicochet && element.Id == Engine.Elements.RicochetId)
            {
                canRicochet = false;
                actor.Vector = -actor.Vector;
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            if (element.Id == Engine.Elements.BreakableId ||
                element.IsDestructible && (element.Id == Engine.Elements.PlayerId || actor.P1 == 0))
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
                tiles[actor.Location + actor.Vector.Clockwise()].Id == Engine.Elements.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.CounterClockwise();
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            if (canRicochet &&
                tiles[actor.Location + actor.Vector.CounterClockwise()].Id == Engine.Elements.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.Clockwise();
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            Engine.RemoveActor(index);
            Engine.State.ActIndex--;
            if (element.Id == Engine.Elements.ObjectId || element.Id == Engine.Elements.ScrollId)
            {
                Engine.BroadcastLabel(-Engine.Actors.ActorIndexAt(target), Engine.Facts.ShotLabel, false);
            }

            break;
        }
    }
}