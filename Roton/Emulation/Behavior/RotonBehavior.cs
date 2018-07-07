using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class RotonBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly IMover _mover;
        private readonly ICompass _compass;

        public override string KnownName => KnownNames.Roton;

        public RotonBehavior(IActors actors, IRandom random, ITiles tiles, IElements elements,
            IMover mover, ICompass compass) : base(mover)
        {
            _actors = actors;
            _random = random;
            _tiles = tiles;
            _elements = elements;
            _mover = mover;
            _compass = compass;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];

            actor.P3--;
            if (actor.P3 < -actor.P2 % 10)
            {
                actor.P3 = actor.P2 * 10 + _random.Synced(10);
            }

            actor.Vector.CopyFrom(_compass.Seek(actor.Location));
            if (actor.P1 <= _random.Synced(10))
            {
                var temp = actor.Vector.X;
                actor.Vector.X = -actor.P2.Polarity() * actor.Vector.Y;
                actor.Vector.Y = actor.P2.Polarity() * temp;
            }

            var target = actor.Location.Sum(actor.Vector);
            if (_tiles.ElementAt(target).IsFloor)
            {
                _mover.MoveActor(index, target);
            }
            else if (_tiles[target].Id == _elements.PlayerId)
            {
                _mover.Attack(index, target);
            }
        }
    }
}