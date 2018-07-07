using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class RuffianBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly IRandom _random;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly IMover _mover;
        private readonly ICompass _compass;

        public override string KnownName => KnownNames.Ruffian;

        public RuffianBehavior(IActors actors, IEngine engine, IRandom random, ITiles tiles, IElements elements,
            IMover mover, ICompass compass) : base(mover)
        {
            _actors = actors;
            _engine = engine;
            _random = random;
            _tiles = tiles;
            _elements = elements;
            _mover = mover;
            _compass = compass;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= _random.Synced(17))
                {
                    if (actor.P1 >= _random.Synced(9))
                    {
                        actor.Vector.CopyFrom(_compass.Seek(actor.Location));
                    }
                    else
                    {
                        actor.Vector.CopyFrom(_compass.Rnd());
                    }
                }
            }
            else
            {
                if (actor.Location.X == _actors.Player.Location.X || actor.Location.Y == _actors.Player.Location.Y)
                {
                    if (actor.P1 >= _random.Synced(9))
                    {
                        actor.Vector.CopyFrom(_compass.Seek(actor.Location));
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (_tiles.ElementAt(target).Id == _elements.PlayerId)
                {
                    _mover.Attack(index, target);
                }
                else if (_tiles.ElementAt(target).IsFloor)
                {
                    _mover.MoveActor(index, target);
                    if (actor.P2 + 8 <= _random.Synced(17))
                    {
                        actor.Vector.SetTo(0, 0);
                    }
                }
                else
                {
                    actor.Vector.SetTo(0, 0);
                }
            }
        }
    }
}