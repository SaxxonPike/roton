using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class BulletBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly ISounds _sounds;
        private readonly IWorld _world;
        private readonly IState _state;
        private readonly ITiles _tiles;
        private readonly IBroadcaster _broadcaster;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Bullet;

        public BulletBehavior(IActors actors, IElements elements, ISounds sounds, IWorld world,
            IState state, ITiles tiles, IBroadcaster broadcaster, ISounder sounder, IHud hud, IMover mover) : base(mover)
        {
            _actors = actors;
            _elements = elements;
            _sounds = sounds;
            _world = world;
            _state = state;
            _tiles = tiles;
            _broadcaster = broadcaster;
            _sounder = sounder;
            _hud = hud;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = _tiles.ElementAt(target);
                if (element.IsFloor || element.Id == _elements.WaterId)
                {
                    _mover.MoveActor(index, target);
                    break;
                }

                if (canRicochet && element.Id == _elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    _sounder.Play(1, _sounds.Ricochet);
                    continue;
                }

                if (element.Id == _elements.BreakableId ||
                    (element.IsDestructible && (element.Id == _elements.PlayerId || actor.P1 == 0)))
                {
                    if (element.Points != 0)
                    {
                        _world.Score += element.Points;
                        _hud.UpdateStatus();
                    }

                    _mover.Attack(index, target);
                    break;
                }

                if (canRicochet &&
                    _tiles[actor.Location.Sum(actor.Vector.Clockwise())].Id == _elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    _sounder.Play(1, _sounds.Ricochet);
                    continue;
                }

                if (canRicochet &&
                    _tiles[actor.Location.Sum(actor.Vector.CounterClockwise())].Id == _elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    _sounder.Play(1, _sounds.Ricochet);
                    continue;
                }

                _mover.RemoveActor(index);
                _state.ActIndex--;
                if (element.Id == _elements.ObjectId || element.Id == _elements.ScrollId)
                {
                    _broadcaster.BroadcastLabel(-_actors.ActorIndexAt(target), KnownLabels.Shot, false);
                }

                break;
            }
        }
    }
}