using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IConveyor
{
    void Convey(Location center, int direction);
}