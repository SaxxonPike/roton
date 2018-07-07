using System.Collections.Generic;
using System.Linq;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class TigerBehavior : LionBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IRandom _random;
        private readonly ISpawner _spawner;
        public override string KnownName => KnownNames.Tiger;

        public TigerBehavior(IActors actors, IElements elements, IRandom random, ISpawner spawner, IMover mover,
            ITiles tiles, ICompass compass) : base(random, tiles, actors, elements, compass, mover)
        {
            _actors = actors;
            _elements = elements;
            _random = random;
            _spawner = spawner;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var firingElement = _elements.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = _elements.StarId;
            }

            if ((actor.P2 & 0x7F) > 3 * _random.Synced(10))
            {
                var shot = actor.Location.X.AbsDiff(_actors.Player.Location.X) <= 2 &&
                           _spawner.SpawnProjectile(firingElement, actor.Location,
                               new Vector(0, (_actors.Player.Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(_actors.Player.Location.Y) <= 2)
                {
                    _spawner.SpawnProjectile(firingElement, actor.Location,
                        new Vector((_actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            // Proceed to lion code.
            base.Act(index);
        }
    }
}