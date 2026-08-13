using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
public sealed class DuplicatorDraw : IDraw
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public DuplicatorDraw(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public AnsiChar Draw(IXyPair location)
    {
        return Engine.ActorAt(location).P1 switch
        {
            2 => new AnsiChar(0xF9, Engine.Tiles[location].Color),
            3 => new AnsiChar(0xF8, Engine.Tiles[location].Color),
            4 => new AnsiChar(0x6F, Engine.Tiles[location].Color),
            5 => new AnsiChar(0x4F, Engine.Tiles[location].Color),
            _ => new AnsiChar(0xFA, Engine.Tiles[location].Color)
        };
    }
}