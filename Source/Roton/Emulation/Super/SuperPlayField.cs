using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperPlayField(
    IState state,
    ITerminal terminal,
    IBoard board)
    : PlayField
{
    public override void DrawTile(int x, int y, AnsiChar ac)
    {
        if (state.EditorMode)
        {
            if (x is >= 0 and < 96 && y is >= 0 and < 80)
            {
                terminal.Plot(x, y, ac);
            }
        }
        else
        {
            var loc = new Location(x, y) + GetTranslation();
            if (IsWithinCamera(loc))
                terminal.Plot(loc.X, loc.Y, ac);
        }
    }

    private static bool IsWithinCamera(Location loc) =>
        loc.X is >= 0x0E and <= 0x25 && loc.Y is >= 0x02 and <= 0x15;

    private Vector GetTranslation() =>
        new(0x0F + -board.Camera.X, 0x03 + -board.Camera.Y);
}