using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3F)]
internal sealed class WebDraw(
    ITiles tiles,
    IState state,
    IElementList elementList,
    IFeatures features)
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(state.WebChars[features.GetAdjacent(location, elementList.WebId)],
            tiles[location].Color);
    }
}