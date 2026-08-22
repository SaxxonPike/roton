using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
public sealed class StarDraw(
    IEngineAccessor engine,
    ITiles tiles,
    IState state)
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        var tileColor = tiles[location].Color;
        tileColor++;
        if (tileColor > 15)
            tileColor = 9;
        tiles[location].Color = tileColor;
        return new AnsiChar(state.StarChars[state.GameCycle & 0x3], tileColor);
    }
}