using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IConveyor
{
    /// <remarks>
    /// RoZ: ElementConveyorTick
    /// </remarks>
    void Convey(Location center, int direction);
}