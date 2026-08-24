using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IRadiusUpdater
{
    void UpdateRadius(Location location, RadiusMode mode);
}