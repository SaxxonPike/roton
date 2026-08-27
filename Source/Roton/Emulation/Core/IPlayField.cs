using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPlayField
{
    void DrawTile(int x, int y, AnsiChar ac);
}