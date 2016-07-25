using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class SpiderBehavior : EnemyBehavior
    {
        public override string KnownName => "Spider";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 <= engine.RandomNumberDeterministic(10)
                ? engine.Rnd()
                : engine.Seek(actor.Location));

            if (!ActSpiderAttemptDirection(engine, index, vector))
            {
                var i = (engine.RandomNumberDeterministic(2) << 1) - 1;
                if (!ActSpiderAttemptDirection(engine, index, vector.Product(i).Swap()))
                {
                    if (!ActSpiderAttemptDirection(engine, index, vector.Product(i).Swap().Opposite()))
                    {
                        ActSpiderAttemptDirection(engine, index, vector.Opposite());
                    }
                }
            }
        }

        private static bool ActSpiderAttemptDirection(IEngine engine, int index, IXyPair vector)
        {
            var actor = engine.Actors[index];
            var target = actor.Location.Sum(vector);
            var targetElement = engine.ElementAt(target).Id;

            if (targetElement == engine.Elements.WebId)
            {
                engine.MoveActor(index, target);
                return true;
            }

            if (targetElement == engine.Elements.PlayerId)
            {
                engine.Attack(index, target);
                return true;
            }

            return false;
        }
    }
}
