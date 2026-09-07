namespace Roton.Emulation.Core;

public interface IStatistics
{
    /// <remarks>
    /// RoZ: GameUpdateSidebar (memory available only)
    /// </remarks>
    int CalculateMemoryUsage();
}