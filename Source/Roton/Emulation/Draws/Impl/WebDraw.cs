using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3F)]
public sealed class WebDraw(
    IEngineAccessor engine,
    ITiles tiles,
    IState state,
    IElementList elementList)
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(state.WebChars[Engine.Adjacent(location, elementList.WebId)],
            tiles[location].Color);
    }
}