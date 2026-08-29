using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "DARK")]
internal sealed class DarkCheat(
    IBoard board,
    IHud hud)
    : ICheat
{
    public void Execute(bool clear)
    {
        board.IsDark = !clear;
        hud.RedrawBoard();
    }
}