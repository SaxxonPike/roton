using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
public sealed class StarAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];

        actor.P2 = (actor.P2 - 1) & 0xFF;
        if (actor.P2 > 0)
        {
            if ((actor.P2 & 1) == 0)
            {
                actor.Vector = Engine.Seek(actor.Location);
                var targetLocation = actor.Location + actor.Vector;
                var targetElement = Engine.Tiles.ElementAt(targetLocation);

                if (targetElement.Id == Engine.ElementList.PlayerId || targetElement.Id == Engine.ElementList.BreakableId)
                {
                    Engine.Attack(index, targetLocation);
                }
                else
                {
                    if (!targetElement.IsFloor)
                    {
                        Engine.Push(targetLocation, actor.Vector);
                    }

                    if (targetElement.IsFloor || targetElement.Id == Engine.ElementList.WaterId)
                    {
                        Engine.MoveActor(index, targetLocation);
                    }
                }
            }
            else
            {
                Engine.UpdateBoard(actor.Location);
            }
        }
        else
        {
            Engine.RemoveActor(index);
        }
    }
}