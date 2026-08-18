using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
public sealed class ClockwiseConveyorDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return ((Engine.State.GameCycle / Engine.ElementList.Clockwise().Cycle) & 0x3) switch
        {
            0 => new AnsiChar(0xB3, Engine.Tiles[location].Color),
            1 => new AnsiChar(0x2F, Engine.Tiles[location].Color),
            2 => new AnsiChar(0xC4, Engine.Tiles[location].Color),
            _ => new AnsiChar(0x5C, Engine.Tiles[location].Color)
        };
    }
}