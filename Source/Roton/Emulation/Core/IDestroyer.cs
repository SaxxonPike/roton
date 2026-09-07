using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IDestroyer
{
    /// <remarks>
    /// RoZ: BoardDamageTile
    /// </remarks>
    void Destroy(Location target);
}