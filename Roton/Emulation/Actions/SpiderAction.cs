using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.SuperZzt, 0x3E)]
    public sealed class SpiderAction : IAction
    {
        private readonly IEngine _engine;
        
        public SpiderAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 <= _engine.Random.Synced(10)
                ? _engine.Rnd()
                : _engine.Seek(actor.Location));

            if (!ActSpiderAttemptDirection(index, vector))
            {
                var i = (_engine.Random.Synced(2) << 1) - 1;
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
            var actor = _engine.Actors[index];
            var target = actor.Location.Sum(vector);
            var targetElement = _engine.Tiles.ElementAt(target).Id;

            if (targetElement == _engine.ElementList.WebId)
            {
                _engine.MoveActor(index, target);
                return true;
            }

            if (targetElement == _engine.ElementList.PlayerId)
            {
                _engine.Attack(index, target);
                return true;
            }

            return false;
        }
    }
}