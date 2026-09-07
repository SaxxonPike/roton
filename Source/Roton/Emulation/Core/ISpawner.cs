using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ISpawner
{
    /// <remarks>
    /// RoZ: AddStat
    /// </remarks>
    void SpawnActor(Location location, Tile tile, int cycle, IActor? source);
    
    /// <remarks>
    /// RoZ: BoardShoot
    /// </remarks>
    bool SpawnProjectile(int elementId, Location location, Vector vector, bool enemyOwned);
}