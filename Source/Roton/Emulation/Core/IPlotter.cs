using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPlotter
{
    void Plot(Location location, Tile tile);
    void Put(Location location, Vector vector, Tile kind);
}