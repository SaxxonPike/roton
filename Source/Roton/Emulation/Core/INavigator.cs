using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface INavigator
{
    Vector Rnd();
    Vector RndP(Vector vector);
    Vector Seek(Location location);

}