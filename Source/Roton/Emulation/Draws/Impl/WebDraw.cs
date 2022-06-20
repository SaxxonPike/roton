using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3F)]
public sealed class WebDraw : IDraw
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;
        
    public WebDraw(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public AnsiChar Draw(IXyPair location)
    {
        return new AnsiChar(Engine.State.WebChars[Engine.Adjacent(location, Engine.ElementList.WebId)],
            Engine.Tiles[location].Color);
    }
}