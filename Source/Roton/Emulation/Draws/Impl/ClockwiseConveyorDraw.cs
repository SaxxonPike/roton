using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
public sealed class ClockwiseConveyorDraw(
    IState state,
    IElementList elementList,
    ITiles tiles)
    : IDraw
{
    public AnsiChar Draw(Location location) =>
        ((state.GameCycle / elementList.Clockwise().Cycle) & 0x3) switch
        {
            0 => new AnsiChar(0xB3, tiles[location].Color),
            1 => new AnsiChar(0x2F, tiles[location].Color),
            2 => new AnsiChar(0xC4, tiles[location].Color),
            _ => new AnsiChar(0x5C, tiles[location].Color)
        };
}