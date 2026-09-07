using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPlotter
{
    /// <remarks>
    /// RoZ: OopPlaceTile
    /// </remarks>
    void Plot(Location location, Tile tile);

    /// <remarks>
    /// RoZ: OopExecute (#PUT command)
    /// </remarks>
    void Put(Location location, Vector vector, Tile kind);
}