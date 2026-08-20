using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x40)]
public sealed class StoneDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(0x41 + Engine.Random.GetNext(0x1A), Engine.Tiles[location].Color);
    }
}