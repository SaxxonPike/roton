using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IDestroyer
{
    void Destroy(Location target);
}