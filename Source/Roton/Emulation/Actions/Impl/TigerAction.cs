using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x2A)]
    [Context(Context.Super, 0x2A)]
    public sealed class TigerAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public TigerAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            var firingElement = Engine.ElementList.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = Engine.ElementList.StarId;
            }

            if ((actor.P2 & 0x7F) > 3 * Engine.Random.GetNext(10))
            {
                var shot = actor.Location.X.AbsDiff(Engine.Actors.Player.Location.X) <= 2 &&
                           Engine.SpawnProjectile(firingElement, actor.Location,
                               new Vector(0, (Engine.Actors.Player.Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(Engine.Actors.Player.Location.Y) <= 2)
                {
                    Engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector((Engine.Actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            // Proceed to lion code.
            Engine.ActionList.Get(Engine.ElementList.LionId).Act(index);
        }
    }
}