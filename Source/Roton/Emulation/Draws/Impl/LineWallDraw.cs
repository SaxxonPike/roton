using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1F)]
[Context(Context.Super, 0x1F)]
internal sealed class LineWallDraw(
    IState state,
    IElementList elements,
    ITiles tiles,
    IAdjacentFinder adjacentFinder)
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(state.LineChars[adjacentFinder.GetAdjacent(location, elements.LineId)],
            tiles[location].Color);
    }
}