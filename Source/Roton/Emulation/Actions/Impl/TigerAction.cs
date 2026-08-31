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
    IEngineAccessor engine,
    IActorList actorList,
    IElementList elementList,
    IRandomizer randomizer,
    IActionList actionList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var firingElement = elementList.BulletId;

        if (actor.P2 >= 0x80) 
            firingElement = elementList.StarId;

        if ((actor.P2 & 0x7F) > 3 * randomizer.GetNext(10))
        {
            var shot = actor.Location.X.AbsDiff(actorList.Player.Location.X) <= 2 &&
                       Engine.SpawnProjectile(firingElement, actor.Location,
                           new Vector(0, (actorList.Player.Location.Y - actor.Location.Y).Polarity()), true);

            if (!shot && actor.Location.Y.AbsDiff(actorList.Player.Location.Y) <= 2)
            {
                Engine.SpawnProjectile(firingElement, actor.Location,
                    new Vector((actorList.Player.Location.X - actor.Location.X).Polarity(), 0), true);
            }
        }

        // Proceed to lion code.
        actionList.Get(elementList.LionId)?.Act(index);
    }
}