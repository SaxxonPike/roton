using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public static class AdjacentFinderExtensions
{
    public static int GetAdjacent(this IAdjacentFinder adjacentFinder, Location location, int id) =>
        (adjacentFinder.TestAdjacent(location + Vector.North, id) ? 1 : 0) |
        (adjacentFinder.TestAdjacent(location + Vector.South, id) ? 2 : 0) |
        (adjacentFinder.TestAdjacent(location + Vector.West, id) ? 4 : 0) |
        (adjacentFinder.TestAdjacent(location + Vector.East, id) ? 8 : 0);

}