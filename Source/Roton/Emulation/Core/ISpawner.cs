using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ISpawner
{
    void SpawnActor(Location location, Tile tile, int cycle, IActor? source);
    bool SpawnProjectile(int elementId, Location location, Vector vector, bool enemyOwned);
}