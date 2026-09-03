using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface INavigator
{
    /// <remarks>
    /// RoZ: CalcDirectionRnd
    /// </remarks>
    Vector Rnd();

    /// <remarks>
    /// RoZ: CalcDirectionRndP
    /// </remarks>
    Vector RndP(Vector vector);

    /// <remarks>
    /// RoZ: CalcDirectionSeek
    /// </remarks>
    Vector Seek(Location location);
}