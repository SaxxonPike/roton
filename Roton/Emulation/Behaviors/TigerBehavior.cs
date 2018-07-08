using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class TigerBehavior : LionBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Tiger;

        public TigerBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            var firingElement = _engine.Elements.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = _engine.Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > 3 * _engine.Random.Synced(10))
            {
                var shot = actor.Location.X.AbsDiff(_engine.Actors.Player.Location.X) <= 2 &&
                           _engine.SpawnProjectile(firingElement, actor.Location,
                               new Vector(0, (_engine.Actors.Player.Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(_engine.Actors.Player.Location.Y) <= 2)
                {
                    _engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector((_engine.Actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            // Proceed to lion code.
            base.Act(index);
        }
    }
}