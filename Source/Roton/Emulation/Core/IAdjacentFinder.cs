using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IAdjacentFinder
{
    bool TestAdjacent(Location location, int id);
}