using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
public sealed class PusherDraw(Lazy<IEngine> engine) : IDraw
{
    private IEngine Engine => engine.Value;

    public AnsiChar Draw(IXyPair location)
    {
        var actor = Engine.ActorAt(location);
        return actor.Vector.X switch
        {
            1 => new AnsiChar(0x10, Engine.Tiles[location].Color),
            -1 => new AnsiChar(0x11, Engine.Tiles[location].Color),
            _ => actor.Vector.Y == -1
                ? new AnsiChar(0x1E, Engine.Tiles[location].Color)
                : new AnsiChar(0x1F, Engine.Tiles[location].Color)
        };
    }
}