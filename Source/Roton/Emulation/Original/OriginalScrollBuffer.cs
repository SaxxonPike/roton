using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalScrollBuffer(
    IFacts facts,
    // IBoardUpdater boardUpdater,
    ITerminal terminal)
    : IScrollBuffer
{
    // private readonly int _left = facts.ScrollLeft;
    // private readonly int _right = facts.ScrollLeft + facts.ScrollWidth;
    //
    // public void Capture()
    // {
    //     // The original engine does not capture the tiles behind the scroll.
    // }
    //
    // public void Restore(int y)
    // {
    //     for (var x = _left; x < _right; x++)
    //         boardUpdater.UpdateBoard(new Location(x + 1, y + 1));
    // }
    private readonly int _left = facts.ScrollLeft;
    private readonly int _top = facts.ScrollTop;
    private readonly int _width = facts.ScrollWidth;
    private readonly int _height = facts.ScrollHeight;
    private readonly AnsiChar[] _buffer = new AnsiChar[facts.ScrollWidth * facts.ScrollHeight];

    public void Capture()
    {
        var i = 0;

        for (var y = 0; y < _height; y++)
        for (var x = 0; x < _width; x++)
            _buffer[i++] = terminal.Read(x + _left, y + _top);
    }

    public void Restore(int y)
    {
        var i = _width * (y - _top);
        for (var x = _left; x < _left + _width; x++)
            terminal.Plot(x, y, _buffer[i++]);
    }
}