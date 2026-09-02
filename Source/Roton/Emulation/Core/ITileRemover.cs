using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITileRemover
{
    void RemoveActor(Location location, int index, Tile tile);
    void RemoveItem(Location location);
}