using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPusher
{
    /// <remarks>
    /// RoZ: ElementPushablePush
    /// </remarks>
    void Push(Location location, Vector vector);

    /// <remarks>
    /// RoZ: ElementTransporterMove
    /// </remarks>
    void Transport(Location location, Vector vector);
}