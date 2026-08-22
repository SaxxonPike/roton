using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
public sealed class ObjectDraw(
    IEngineAccessor engine,
    ITiles tiles) 
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(Engine.ActorAt(location).P1, tiles[location].Color);
    }
}