using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalPlayField(
    ITerminal terminal)
    : PlayField
{
    public override void DrawTile(int x, int y, AnsiChar ac) => 
        terminal.Plot(x, y, ac);
}