using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalFadeMatrix(
    IEngineAccessor engine,
    ITerminal terminal,
    IRandomizer randomizer,
    IBoardUpdater boardUpdater)
    : FadeMatrix(engine, randomizer, 0, 0, 60, 25, 0x80)
{
    protected override void DrawAt(int x, int y, AnsiChar ac) =>
        terminal.Plot(x, y, ac);

    protected override void RedrawAt(int x, int y)
    {
        var location = new Location(x + 1, y + 1);
        terminal.Plot(x, y, boardUpdater.Draw(location));
    }
}