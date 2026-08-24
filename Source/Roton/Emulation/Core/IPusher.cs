using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPusher
{
    void Push(Location location, Vector vector);
    void Transport(Location location, Vector vector);
}