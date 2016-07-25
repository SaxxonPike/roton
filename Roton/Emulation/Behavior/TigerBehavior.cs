using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class TigerBehavior : LionBehavior
    {
        public override string KnownName => "Tiger";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var firingElement = engine.Elements.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = engine.Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > 3 * engine.RandomNumberDeterministic(10))
            {
                var shot = actor.Location.X.AbsDiff(engine.Player.Location.X) <= 2 &&
                           engine.SpawnProjectile(firingElement, actor.Location,
                               new Vector(0, (engine.Player.Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(engine.Player.Location.Y) <= 2)
                {
                    engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector((engine.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            // Proceed to lion code.
            base.Act(engine, index);
        }
    }
}
