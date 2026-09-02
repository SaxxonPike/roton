using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperScrollBuffer(
    ITerminal terminal,
    IFacts facts)
    : IScrollBuffer
{
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