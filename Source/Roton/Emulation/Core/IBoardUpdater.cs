using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IBoardUpdater
{
    AnsiChar Draw(Location location);
    void UpdateBoard(Location location);
}