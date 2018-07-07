using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class SpiderBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly ICompass _compass;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Spider;

        public SpiderBehavior(IActors actors, IRandom random, ITiles tiles, IElements elements, ICompass compass,
            IMover mover) : base(mover)
        {
            _actors = actors;
            _random = random;
            _tiles = tiles;
            _elements = elements;
            _compass = compass;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 <= _random.Synced(10)
                ? _compass.Rnd()
                : _compass.Seek(actor.Location));

            if (!ActSpiderAttemptDirection(index, vector))
            {
                var i = (_random.Synced(2) << 1) - 1;
                if (!ActSpiderAttemptDirection(index, vector.Product(i).Swap()))
                {
                    if (!ActSpiderAttemptDirection(index, vector.Product(i).Swap().Opposite()))
                    {
                        ActSpiderAttemptDirection(index, vector.Opposite());
                    }
                }
            }
        }

        private bool ActSpiderAttemptDirection(int index, IXyPair vector)
        {
            var actor = _actors[index];
            var target = actor.Location.Sum(vector);
            var targetElement = _tiles.ElementAt(target).Id;

            if (targetElement == _elements.WebId)
            {
                _mover.MoveActor(index, target);
                return true;
            }

            if (targetElement == _elements.PlayerId)
            {
                _mover.Attack(index, target);
                return true;
            }

            return false;
        }
    }
}