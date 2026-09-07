using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMover
{
    /// <remarks>
    /// RoZ: MoveStat
    /// </remarks>
    void Move(int index, Location location);
    
    /// <remarks>
    /// RoZ: ElementApplyMovement
    /// </remarks>
    void Float(int index);
}