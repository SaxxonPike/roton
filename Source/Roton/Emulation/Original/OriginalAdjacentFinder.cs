using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalAdjacentFinder(
    ITiles tiles,
    IElementList elements)
    : AdjacentFinder
{
    protected override bool TestAdjacent(Location location, int id)
    {
        var eId = tiles[location].Id;
        return eId == id || eId == elements.BoardEdgeId;
    }
}