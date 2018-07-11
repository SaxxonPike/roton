using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x27)]
    [ContextEngine(ContextEngine.SuperZzt, 0x27)]
    public sealed class SpinningGunAction : IAction
    {
        private readonly IEngine _engine;
        
        public string KnownName => KnownNames.SpinningGun;

        public SpinningGunAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var firingElement = _engine.ElementList.BulletId;
            var shot = false;

            _engine.UpdateBoard(actor.Location);

            if (actor.P2 >= 0x80)
            {
                firingElement = _engine.ElementList.StarId;
            }

            if ((actor.P2 & 0x7F) > _engine.Random.Synced(9))
            {
                if (actor.P1 >= _engine.Random.Synced(9))
                {
                    if (actor.Location.X.AbsDiff(_engine.Actors.Player.Location.X) <= 2)
                    {
                        shot = _engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector(0, (_engine.Actors.Player.Location.Y - actor.Location.Y).Polarity()), true);
                    }

                    if (!shot && actor.Location.Y.AbsDiff(_engine.Actors.Player.Location.Y) <= 2)
                    {
                        _engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector((_engine.Actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                    }
                }
                else
                {
                    _engine.SpawnProjectile(firingElement, actor.Location, _engine.Rnd(), true);
                }
            }
        }
    }
}