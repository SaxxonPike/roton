using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
internal sealed class SpinningGunDraw(
    ITiles tiles,
    IState state)
    : IDraw
{
    public AnsiChar Draw(Location location) =>
        (state.GameCycle & 0x7) switch
        {
            0 or 1 => new AnsiChar(0x18, tiles[location].Color),
            2 or 3 => new AnsiChar(0x1A, tiles[location].Color),
            4 or 5 => new AnsiChar(0x19, tiles[location].Color),
            _ => new AnsiChar(0x1B, tiles[location].Color)
        };
}