using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x2D)]
[Context(Context.Super, 0x2D)]
public sealed class CentipedeSegmentAction(
    IEngineAccessor engine,
    IActorList actorList,
    ITiles tiles,
    IElementList elementList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        if (actor.Leader < 0)
        {
            if (actor.Leader < -1)
            {
                tiles[actor.Location].Id = elementList.HeadId;
            }
            else
            {
                actor.Leader--;
            }
        }
    }
}