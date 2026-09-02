using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITileFinder
{
    bool Find(Tile kind, Location location);
}