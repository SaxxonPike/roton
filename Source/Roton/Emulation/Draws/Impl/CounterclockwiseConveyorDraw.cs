using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x11)]
[Context(Context.Super, 0x11)]
public sealed class CounterclockwiseConveyorDraw(
    IEngineAccessor engine,
    ITiles tiles,
    IElementList elementList,
    IState state)
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return ((state.GameCycle / elementList.Counter().Cycle) & 0x3) switch
        {
            3 => new AnsiChar(0xB3, tiles[location].Color),
            2 => new AnsiChar(0x2F, tiles[location].Color),
            1 => new AnsiChar(0xC4, tiles[location].Color),
            _ => new AnsiChar(0x5C, tiles[location].Color)
        };
    }
}