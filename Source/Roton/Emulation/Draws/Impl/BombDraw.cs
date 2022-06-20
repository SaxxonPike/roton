using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public sealed class BombDraw : IDraw
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public BombDraw(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public AnsiChar Draw(IXyPair location)
    {
        var p1 = Engine.ActorAt(location).P1;
        return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, Engine.Tiles[location].Color);
    }
}