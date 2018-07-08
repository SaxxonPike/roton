﻿using Roton.Core;
using Roton.Emulation.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class SpiderBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Spider;

        public SpiderBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
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

            if (targetElement == _engine.Elements.WebId)
            {
                _engine.MoveActor(index, target);
                return true;
            }

            if (targetElement == _engine.Elements.PlayerId)
            {
                _engine.Attack(index, target);
                return true;
            }

            return false;
        }
    }
}