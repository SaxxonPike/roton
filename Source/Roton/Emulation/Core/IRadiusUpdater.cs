using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IRadiusUpdater
{
    /// <remarks>
    /// RoZ: DrawPlayerSurroundings
    /// </remarks>
    void UpdateRadius(Location location, RadiusMode mode);
}