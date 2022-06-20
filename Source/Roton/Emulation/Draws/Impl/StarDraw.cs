using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
public sealed class StarDraw : IDraw
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public StarDraw(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public AnsiChar Draw(IXyPair location)
    {
        var tile = Engine.Tiles[location];
        tile.Color++;
        if (tile.Color > 15)
            tile.Color = 9;
        return new AnsiChar(Engine.State.StarChars[Engine.State.GameCycle & 0x3], tile.Color);
    }
}