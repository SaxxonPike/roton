using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BulletBehavior : EnemyBehavior
    {
        private readonly IActorList _actorList;
        private readonly IEngine _engine;
        private readonly IElementList _elementList;
        private readonly ISoundSet _soundSet;
        private readonly IWorld _world;
        private readonly IState _state;

        public override string KnownName => KnownNames.Bullet;

        public BulletBehavior(IActorList actorList, IEngine engine, IElementList elementList, ISoundSet soundSet, IWorld world, IState state)
        {
            _actorList = actorList;
            _engine = engine;
            _elementList = elementList;
            _soundSet = soundSet;
            _world = world;
            _state = state;
        }
        
        public override void Act(int index)
        {
            var actor = _actorList[index];
            var canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = _engine.ElementAt(target);
                if (element.IsFloor || element.Id == _elementList.WaterId)
                {
                    _engine.MoveActor(index, target);
                    break;
                }
                if (canRicochet && element.Id == _elementList.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    _engine.PlaySound(1, _soundSet.Ricochet);
                    continue;
                }
                if (element.Id == _elementList.BreakableId ||
                    (element.IsDestructible && (element.Id == _elementList.PlayerId || actor.P1 == 0)))
                {
                    if (element.Points != 0)
                    {
                        _world.Score += element.Points;
                        _engine.UpdateStatus();
                    }
                    _engine.Attack(index, target);
                    break;
                }
                if (canRicochet &&
                    _engine.TileAt(actor.Location.Sum(actor.Vector.Clockwise())).Id == _elementList.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    _engine.PlaySound(1, _soundSet.Ricochet);
                    continue;
                }
                if (canRicochet &&
                    _engine.TileAt(actor.Location.Sum(actor.Vector.CounterClockwise())).Id == _elementList.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    _engine.PlaySound(1, _soundSet.Ricochet);
                    continue;
                }
                _engine.RemoveActor(index);
                _state.ActIndex--;
                if (element.Id == _elementList.ObjectId || element.Id == _elementList.ScrollId)
                {
                    _engine.BroadcastLabel(-_engine.ActorIndexAt(target), @"SHOT", false);
                }
                break;
            }
        }
    }
}