using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IForestHandler
{
    /// <remarks>
    /// RoZ: ElementForestTouch (tile removal)
    /// </remarks>
    void ClearForest(Location location);
}