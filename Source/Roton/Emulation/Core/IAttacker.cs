using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IAttacker
{
    void Attack(int index, Location location);
    void Destroy(Location target);
    void Harm(int index);
}