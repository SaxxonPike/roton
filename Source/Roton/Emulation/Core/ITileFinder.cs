using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITileFinder
{
    /// <remarks>
    /// RoZ: FindTileOnBoard
    /// </remarks>
    bool Find(Tile kind, Location location);
}