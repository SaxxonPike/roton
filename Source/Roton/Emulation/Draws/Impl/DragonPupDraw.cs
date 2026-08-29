using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3C)]
internal sealed class DragonPupDraw(
    ITiles tiles,
    IState state)
    : IDraw
{
    public AnsiChar Draw(Location location) =>
        (state.GameCycle & 0x3) switch
        {
            0 or 2 => new AnsiChar(0x94, tiles[location].Color),
            1 => new AnsiChar(0xA2, tiles[location].Color),
            _ => new AnsiChar(0x95, tiles[location].Color)
        };
}