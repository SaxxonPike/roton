using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IActorRemover
{
    void RemoveActor(Location location, int index, Tile tile);
}