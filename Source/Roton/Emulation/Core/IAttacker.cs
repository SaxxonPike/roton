using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IAttacker
{
    /// <remarks>
    /// RoZ: BoardAttack
    /// </remarks>
    void Attack(int index, Location location);
}