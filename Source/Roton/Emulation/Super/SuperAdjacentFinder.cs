using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperAdjacentFinder(
    ITiles tiles,
    IElementList elements,
    IActorList actors)
    : AdjacentFinder
{
    protected override bool TestAdjacent(Location location, int id)
    {
        var eId = tiles[location].Id;
        if (eId == id || eId == elements.BoardEdgeId)
            return true;

        if (tiles.ElementAt(location).Cycle >= 0)
        {
            eId = actors.ActorAt(location).UnderTile.Id;
            if (eId == id || eId == elements.BoardEdgeId)
                return true;
        }

        return false;
    }
}