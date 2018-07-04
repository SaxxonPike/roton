using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class TigerBehavior : LionBehavior
    {
        public override string KnownName => "Tiger";

        public override void Act(int index)
        {
            var actor = _actorList[index];
            var firingElement = engine.Elements.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = engine.Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > 3*engine.SyncRandomNumber(10))
            {
                var shot = actor.Location.X.AbsDiff(_actorList.GetPlayer().Location.X) <= 2 &&
                           engine.SpawnProjectile(firingElement, actor.Location,
                               new Vector(0, (_actorList.GetPlayer().Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(_actorList.GetPlayer().Location.Y) <= 2)
                {
                    engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector((_actorList.GetPlayer().Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            // Proceed to lion code.
            base.Act(index);
        }
    }
}