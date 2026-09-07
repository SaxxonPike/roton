using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IBoardUpdater
{
    /// <remarks>
    /// RoZ: BoardDrawTile (only returns char+color)
    /// </remarks>
    AnsiChar Draw(Location location);

    /// <remarks>
    /// RoZ: BoardDrawTile (w/screen update)
    /// </remarks>
    void UpdateBoard(Location location);
}