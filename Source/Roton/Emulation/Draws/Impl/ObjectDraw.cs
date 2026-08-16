using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
public sealed class ObjectDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(IXyPair location)
    {
        return new AnsiChar(Engine.ActorAt(location).P1, Engine.Tiles[location].Color);
    }
}