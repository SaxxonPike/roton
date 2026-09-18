using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Torch.App.Impl;

public class TorchPlayField(ITerminal terminal)
    : IPlayField
{
    public void DrawTile(int x, int y, AnsiChar ac) => 
        terminal.Plot(x, y, ac);

}