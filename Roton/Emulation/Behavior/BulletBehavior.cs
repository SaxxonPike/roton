using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BulletBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly IElements _elements;
        private readonly ISounds _sounds;
        private readonly IWorld _world;
        private readonly IState _state;
        private readonly IGrid _grid;

        public override string KnownName => KnownNames.Bullet;

        public BulletBehavior(IActors actors, IEngine engine, IElements elements, ISounds sounds, IWorld world, IState state, IGrid grid) : base(engine)
        {
            _actors = actors;
            _engine = engine;
            _elements = elements;
            _sounds = sounds;
            _world = world;
            _state = state;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            var canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = _grid.ElementAt(target);
                if (element.IsFloor || element.Id == _elements.WaterId)
                {
                    _engine.MoveActor(index, target);
                    break;
                }
                if (canRicochet && element.Id == _elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    _engine.PlaySound(1, _sounds.Ricochet);
                    continue;
                }
                if (element.Id == _elements.BreakableId ||
                    (element.IsDestructible && (element.Id == _elements.PlayerId || actor.P1 == 0)))
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
                    _grid[actor.Location.Sum(actor.Vector.Clockwise())].Id == _elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    _engine.PlaySound(1, _sounds.Ricochet);
                    continue;
                }
                if (canRicochet &&
                    _grid[actor.Location.Sum(actor.Vector.CounterClockwise())].Id == _elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    _engine.PlaySound(1, _sounds.Ricochet);
                    continue;
                }
                _engine.RemoveActor(index);
                _state.ActIndex--;
                if (element.Id == _elements.ObjectId || element.Id == _elements.ScrollId)
                {
                    _engine.BroadcastLabel(-_actors.ActorIndexAt(target), KnownLabels.Shot, false);
                }
                break;
            }
        }
    }
}