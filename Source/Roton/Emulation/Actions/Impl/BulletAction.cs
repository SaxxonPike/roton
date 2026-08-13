using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x12)]
[Context(Context.Super, 0x45)]
public sealed class BulletAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public BulletAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var canRicochet = true;
        while (true)
        {
            var target = actor.Location.Sum(actor.Vector);
            var element = Engine.Tiles.ElementAt(target);
            if (element.IsFloor || element.Id == Engine.ElementList.WaterId)
            {
                Engine.MoveActor(index, target);
                break;
            }

            if (canRicochet && element.Id == Engine.ElementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector.SetOpposite();
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
                Engine.Tiles[actor.Location.Sum(actor.Vector.Clockwise())].Id == Engine.ElementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector.SetCounterClockwise();
                Engine.PlaySound(1, Engine.Sounds.Ricochet);
                continue;
            }

            if (canRicochet &&
                Engine.Tiles[actor.Location.Sum(actor.Vector.CounterClockwise())].Id == Engine.ElementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector.SetClockwise();
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