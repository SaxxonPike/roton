using Roton.Emulation.Data;

namespace Roton.Emulation.Core.Impl;

public abstract class AdjacentFinder : IAdjacentFinder
{
    public int GetAdjacent(Location location, int id) =>
        (TestAdjacent(location + Vector.North, id) ? 1 : 0) |
        (TestAdjacent(location + Vector.South, id) ? 2 : 0) |
        (TestAdjacent(location + Vector.West, id) ? 4 : 0) |
        (TestAdjacent(location + Vector.East, id) ? 8 : 0);

    protected abstract bool TestAdjacent(Location location, int id);
}