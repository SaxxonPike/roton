using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
public sealed class SpinningGunAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public SpinningGunAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var firingElement = Engine.ElementList.BulletId;
        var shot = false;

        Engine.UpdateBoard(actor.Location);

        if (actor.P2 >= 0x80)
        {
            firingElement = Engine.ElementList.StarId;
        }

        if ((actor.P2 & 0x7F) > Engine.Random.GetNext(9))
        {
            if (actor.P1 >= Engine.Random.GetNext(9))
            {
                if (actor.Location.X.AbsDiff(Engine.Actors.Player.Location.X) <= 2)
                {
                    shot = Engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector(0, (Engine.Actors.Player.Location.Y - actor.Location.Y).Polarity()), true);
                }

                if (!shot && actor.Location.Y.AbsDiff(Engine.Actors.Player.Location.Y) <= 2)
                {
                    Engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector((Engine.Actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }
            else
            {
                Engine.SpawnProjectile(firingElement, actor.Location, Engine.Rnd(), true);
            }
        }
    }
}