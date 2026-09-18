using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;

namespace Torch.App.Impl;

public class TorchFadeMatrix(
    IRandomizer randomizer,
    IScheduler scheduler,
    IFacts facts,
    ITerminal terminal,
    IBoardUpdater boardUpdater)
    : FadeMatrix(randomizer, scheduler, 0, 0, facts.BoardWidth, facts.BoardHeight, 0)
{
    protected override void DrawAt(int x, int y, AnsiChar ac) =>
        terminal.Plot(x, y, ac);

    protected override void RedrawAt(int x, int y)
    {
        var location = new Location(x + 1, y + 1);
        terminal.Plot(x, y, boardUpdater.Draw(location));
    }
}