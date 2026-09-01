using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalPlayField(
    ITerminal terminal)
    : IPlayField
{
    public void DrawTile(int x, int y, AnsiChar ac) => 
        terminal.Plot(x, y, ac);
}