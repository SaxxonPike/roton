using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3F)]
public sealed class WebDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(Engine.State.WebChars[Engine.Adjacent(location, Engine.ElementList.WebId)],
            Engine.Tiles[location].Color);
    }
}