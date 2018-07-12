using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x2A)]
    [ContextEngine(ContextEngine.Super, 0x2A)]
    public sealed class TigerAction : IAction
    {
        private readonly IEngine _engine;
        
        public TigerAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var firingElement = _engine.ElementList.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = _engine.ElementList.StarId;
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
            _engine.ActionList.Get(_engine.ElementList.LionId).Act(index);
        }
    }
}