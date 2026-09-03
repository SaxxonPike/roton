using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperAdjacentFinder(
    ITiles tiles,
    IElementList elements,
    IActorList actors)
    : IAdjacentFinder
{
    public bool TestAdjacent(Location location, int id) =>
        elements.AreAdjacent(tiles[location].Id, id) ||
        (tiles.ElementAt(location).Cycle >= 0 &&
         elements.AreAdjacent(actors.ActorAt(location).UnderTile.Id, id));
}