using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class BulletBehavior : EnemyBehavior
    {
        public override string KnownName => "Bullet";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = engine.ElementAt(target);
                if (element.IsFloor || element.Id == engine.Elements.WaterId)
                {
                    engine.MoveActor(index, target);
                    break;
                }
                if (canRicochet && element.Id == engine.Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    engine.PlaySound(1, engine.SoundSet.Ricochet);
                    continue;
                }
                if (element.Id == engine.Elements.BreakableId ||
                    (element.IsDestructible && (element.Id == engine.Elements.PlayerId || actor.P1 == 0)))
                {
                    if (element.Points != 0)
                    {
                        engine.WorldData.Score += element.Points;
                        engine.UpdateStatus();
                    }
                    engine.Attack(index, target);
                    break;
                }
                if (canRicochet && engine.TileAt(actor.Location.Sum(actor.Vector.Clockwise())).Id == engine.Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    engine.PlaySound(1, engine.SoundSet.Ricochet);
                    continue;
                }
                if (canRicochet &&
                    engine.TileAt(actor.Location.Sum(actor.Vector.CounterClockwise())).Id == engine.Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    engine.PlaySound(1, engine.SoundSet.Ricochet);
                    continue;
                }
                engine.RemoveActor(index);
                engine.StateData.ActIndex--;
                if (element.Id == engine.Elements.ObjectId || element.Id == engine.Elements.ScrollId)
                {
                    engine.BroadcastLabel(-engine.ActorIndexAt(target), @"SHOT", false);
                }
                break;
            }
        }
    }
}
