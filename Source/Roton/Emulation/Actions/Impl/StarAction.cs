using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
public sealed class StarAction(
    IEngineAccessor engine,
    IActorList actorList,
    IElementList elementList,
    ITiles tiles,
    IBoardUpdater boardUpdater,
    IPusher pusher)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];

        actor.P2 = unchecked((byte)((actor.P2 - 1) & 0xFF));
        if (actor.P2 > 0)
        {
            if ((actor.P2 & 1) == 0)
            {
                actor.Vector = Engine.Seek(actor.Location);
                var targetLocation = actor.Location + actor.Vector;
                var targetElement = tiles.ElementAt(targetLocation);

                if (targetElement.Id == elementList.PlayerId || targetElement.Id == elementList.BreakableId)
                {
                    Engine.Attack(index, targetLocation);
                }
                else
                {
                    if (!targetElement.IsFloor)
                    {
                        pusher.Push(targetLocation, actor.Vector);
                    }

                    if (targetElement.IsFloor || targetElement.Id == elementList.WaterId)
                    {
                        Engine.MoveActor(index, targetLocation);
                    }
                }
            }
            else
            {
                boardUpdater.UpdateBoard(actor.Location);
            }
        }
        else
        {
            Engine.RemoveActor(index);
        }
    }
}