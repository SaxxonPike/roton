using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x11)]
[Context(Context.Super, 0x11)]
public sealed class CounterclockwiseConveyorDraw : IDraw
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public CounterclockwiseConveyorDraw(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public AnsiChar Draw(IXyPair location)
    {
        return ((Engine.State.GameCycle / Engine.ElementList.Counter().Cycle) & 0x3) switch
        {
            3 => new AnsiChar(0xB3, Engine.Tiles[location].Color),
            2 => new AnsiChar(0x2F, Engine.Tiles[location].Color),
            1 => new AnsiChar(0xC4, Engine.Tiles[location].Color),
            _ => new AnsiChar(0x5C, Engine.Tiles[location].Color)
        };
    }
}