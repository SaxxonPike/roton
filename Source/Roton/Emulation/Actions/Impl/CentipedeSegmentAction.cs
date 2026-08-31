using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the centipede segment element.
/// </summary>
[Context(Context.Original, 0x2D)]
[Context(Context.Super, 0x2D)]
internal sealed class CentipedeSegmentAction(
    IActorList actors,
    ITiles tiles,
    IElementList elements)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];
        if (actor.Leader < 0)
        {
            if (actor.Leader < -1)
            {
                tiles[actor.Location].Id = elements.HeadId;
            }
            else
            {
                actor.Leader--;
            }
        }
    }
}