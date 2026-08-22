using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1F)]
[Context(Context.Super, 0x1F)]
public sealed class LineWallDraw(
    IEngineAccessor engine,
    IState state,
    IElementList elementList,
    ITiles tiles)
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(state.LineChars[Engine.Adjacent(location, elementList.LineId)],
            tiles[location].Color);
    }
}