using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the spinning gun element.
/// </summary>
[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
internal sealed class SpinningGunAction(
    IEngineAccessor engine,
    IActorList actors,
    IRandomizer randomizer,
    IElementList elements,
    IBoardUpdater boardUpdater,
    ISpawner spawner,
    INavigator navigator)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];
        var firingElement = elements.BulletId;
        var shot = false;

        boardUpdater.UpdateBoard(actor.Location);

        if (actor.P2 >= 0x80)
            firingElement = elements.StarId;

        if ((actor.P2 & 0x7F) <= randomizer.GetNext(9)) 
            return;

        if (actor.P1 >= randomizer.GetNext(9))
        {
            if (actor.Location.X.AbsDiff(actors.Player.Location.X) <= 2)
            {
                shot = spawner.SpawnProjectile(firingElement, actor.Location,
                    new Vector(0, (actors.Player.Location.Y - actor.Location.Y).Polarity()), true);
            }

            if (!shot && actor.Location.Y.AbsDiff(actors.Player.Location.Y) <= 2)
            {
                spawner.SpawnProjectile(firingElement, actor.Location,
                    new Vector((actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
            }
        }
        else
        {
            spawner.SpawnProjectile(firingElement, actor.Location, navigator.Rnd(), true);
        }
    }
}