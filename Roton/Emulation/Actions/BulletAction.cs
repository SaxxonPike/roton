using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x12)]
    [ContextEngine(ContextEngine.SuperZzt, 0x45)]
    public sealed class BulletAction : IAction
    {
        private readonly IEngine _engine;

        public BulletAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = _engine.Tiles.ElementAt(target);
                if (element.IsFloor || element.Id == _engine.ElementList.WaterId)
                {
                    _engine.MoveActor(index, target);
                    break;
                }

                if (canRicochet && element.Id == _engine.ElementList.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    _engine.PlaySound(1, _engine.Sounds.Ricochet);
                    continue;
                }

                if (element.Id == _engine.ElementList.BreakableId ||
                    (element.IsDestructible && (element.Id == _engine.ElementList.PlayerId || actor.P1 == 0)))
                {
                    if (element.Points != 0)
                    {
                        _engine.World.Score += element.Points;
                        _engine.UpdateStatus();
                    }

                    _engine.Attack(index, target);
                    break;
                }

                if (canRicochet &&
                    _engine.Tiles[actor.Location.Sum(actor.Vector.Clockwise())].Id == _engine.ElementList.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    _engine.PlaySound(1, _engine.Sounds.Ricochet);
                    continue;
                }

                if (canRicochet &&
                    _engine.Tiles[actor.Location.Sum(actor.Vector.CounterClockwise())].Id == _engine.ElementList.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    _engine.PlaySound(1, _engine.Sounds.Ricochet);
                    continue;
                }

                _engine.RemoveActor(index);
                _engine.State.ActIndex--;
                if (element.Id == _engine.ElementList.ObjectId || element.Id == _engine.ElementList.ScrollId)
                {
                    _engine.BroadcastLabel(-_engine.Actors.ActorIndexAt(target), KnownLabels.Shot, false);
                }

                break;
            }
        }
    }
}