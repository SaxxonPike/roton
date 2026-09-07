using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IAdjacentFinder
{
    /// <remarks>
    /// RoZ: ElementLineDraw (adjacency check only)
    /// </remarks>
    bool TestAdjacent(Location location, int id);
}