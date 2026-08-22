using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
public sealed class SpinningGunAction(
    IEngineAccessor engine,
    IActorList actorList,
    IRandomizer randomizer,
    IElementList elementList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var firingElement = elementList.BulletId;
        var shot = false;

        Engine.UpdateBoard(actor.Location);

        if (actor.P2 >= 0x80)
        {
            firingElement = elementList.StarId;
        }

        if ((actor.P2 & 0x7F) > randomizer.GetNext(9))
        {
            if (actor.P1 >= randomizer.GetNext(9))
            {
                if (actor.Location.X.AbsDiff(actorList.Player.Location.X) <= 2)
                {
                    shot = Engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector(0, (actorList.Player.Location.Y - actor.Location.Y).Polarity()), true);
                }

                if (!shot && actor.Location.Y.AbsDiff(actorList.Player.Location.Y) <= 2)
                {
                    Engine.SpawnProjectile(firingElement, actor.Location,
                        new Vector((actorList.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }
            else
            {
                Engine.SpawnProjectile(firingElement, actor.Location, Engine.Rnd(), true);
            }
        }
    }
}