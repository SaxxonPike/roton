using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IAdjacentFinder
{
    int GetAdjacent(Location location, int elementId);
}