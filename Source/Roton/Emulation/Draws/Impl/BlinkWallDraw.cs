using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1D)]
[Context(Context.Super, 0x1D)]
public sealed class BlinkWallDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(0xCE, Engine.Tiles[location].Color);
    }
}