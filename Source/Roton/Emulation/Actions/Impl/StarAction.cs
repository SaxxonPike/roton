using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the star element.
/// </summary>
[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
internal sealed class StarAction(
    IEngineAccessor engine,
    IActorList actors,
    IElementList elements,
    ITiles tiles,
    IBoardUpdater boardUpdater,
    IPusher pusher,
    IMover mover,
    INavigator navigator)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];

        actor.P2 = unchecked((byte)((actor.P2 - 1) & 0xFF));

        if (actor.P2 <= 0)
        {
            Engine.RemoveActor(index);
            return;
        }

        if ((actor.P2 & 1) == 0)
        {
            actor.Vector = navigator.Seek(actor.Location);

            var targetLocation = actor.Location + actor.Vector;
            var targetElement = tiles.ElementAt(targetLocation);

            if (targetElement.Id == elements.PlayerId || targetElement.Id == elements.BreakableId)
            {
                Engine.Attack(index, targetLocation);
            }
            else
            {
                if (!targetElement.IsFloor)
                    pusher.Push(targetLocation, actor.Vector);

                if (targetElement.IsFloor || elements.IsWater(targetElement.Id))
                    mover.MoveActor(index, targetLocation);
            }
        }
        else
        {
            boardUpdater.UpdateBoard(actor.Location);
        }
    }
}