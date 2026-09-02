using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalScrollBuffer(
    IFacts facts,
    IBoardUpdater boardUpdater)
    : IScrollBuffer
{
    private readonly int _left = facts.ScrollLeft;
    private readonly int _right = facts.ScrollLeft + facts.ScrollWidth;

    public void Capture()
    {
        // The original engine does not capture the tiles behind the scroll.
    }

    public void Restore(int y)
    {
        for (var x = _left; x < _right; x++)
            boardUpdater.UpdateBoard(new Location(x + 1, y + 1));
    }
}