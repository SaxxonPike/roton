using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class SpiderBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly IGrid _grid;
        private readonly IElements _elements;
        public override string KnownName => KnownNames.Spider;

        public SpiderBehavior(IEngine engine, IActors actors, IRandom random, IGrid grid, IElements elements) : base(engine)
        {
            _engine = engine;
            _actors = actors;
            _random = random;
            _grid = grid;
            _elements = elements;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 <= _random.Synced(10)
                ? _engine.Rnd()
                : _engine.Seek(actor.Location));

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
            var targetElement = _grid.ElementAt(target).Id;

            if (targetElement == _elements.WebId)
            {
                _engine.MoveActor(index, target);
                return true;
            }

            if (targetElement == _elements.PlayerId)
            {
                _engine.Attack(index, target);
                return true;
            }

            return false;
        }
    }
}