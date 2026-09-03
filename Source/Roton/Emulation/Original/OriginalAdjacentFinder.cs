using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalAdjacentFinder(
    ITiles tiles,
    IElementList elements)
    : IAdjacentFinder
{
    public bool TestAdjacent(Location location, int id) => 
        elements.AreAdjacent(tiles[location].Id, id);
}