using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the tiger element.
/// </summary>
[Context(Context.Original, 0x2A)]
[Context(Context.Super, 0x2A)]
internal sealed class TigerAction(
    IActorList actors,
    IElementList elements,
    IRandomizer randomizer,
    IActionList actions,
    ISpawner spawner)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];
        var firingElement = elements.BulletId;

        if (actor.P2 >= 0x80)
            firingElement = elements.StarId;

        if ((actor.P2 & 0x7F) > 3 * randomizer.GetNext(10))
        {
            var shot = actor.Location.X.AbsDiff(actors.Player.Location.X) <= 2 &&
                       spawner.SpawnProjectile(firingElement, actor.Location,
                           new Vector(0, (actors.Player.Location.Y - actor.Location.Y).Polarity()), true);

            if (!shot && actor.Location.Y.AbsDiff(actors.Player.Location.Y) <= 2)
            {
                spawner.SpawnProjectile(firingElement, actor.Location,
                    new Vector((actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
            }
        }

        // Proceed to lion code.
        actions.Get(elements.LionId)?.Act(index);
    }
}