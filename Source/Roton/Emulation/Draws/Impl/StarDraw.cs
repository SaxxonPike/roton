using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
public sealed class StarDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        ref var tile = ref Engine.Tiles[location];
        tile.Color++;
        if (tile.Color > 15)
            tile.Color = 9;
        return new AnsiChar(Engine.State.StarChars[Engine.State.GameCycle & 0x3], tile.Color);
    }
}