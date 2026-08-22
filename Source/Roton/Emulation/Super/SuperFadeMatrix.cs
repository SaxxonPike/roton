using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperFadeMatrix(
    IEngineAccessor engine, 
    ITerminal terminal,
    IRandomizer randomizer,
    IBoard board)
    : FadeMatrix(engine, randomizer, 14, 2, 24, 20, 0x40)
{
    protected override void DrawAt(int x, int y, AnsiChar ac) => 
        terminal.Plot(x + Left, y + Top, ac);

    protected override void RedrawAt(int x, int y)
    {
        var location = new Location(x + board.Camera.X, y + board.Camera.Y);
        terminal.Plot(x + Left, y + Top, Engine.Draw(location));
    }
}