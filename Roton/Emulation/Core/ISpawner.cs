using Roton.Emulation.Data;

namespace Roton.Core
{
    public interface ISpawner
    {
        void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source);
        bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned);
    }
}