using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterDraw : IDraw
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public TransporterDraw(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public AnsiChar Draw(IXyPair location)
    {
        var actor = Engine.ActorAt(location);

        var index = actor.Cycle > 0 
            ? (Engine.State.GameCycle / actor.Cycle) & 0x3 
            : 0;
                
        if (actor.Vector.X == 0)
        {
            index += (actor.Vector.Y << 1) + 2;
            return new AnsiChar(Engine.State.TransporterVChars[index], Engine.Tiles[location].Color);
        }

        index += (actor.Vector.X << 1) + 2;
        return new AnsiChar(Engine.State.TransporterHChars[index], Engine.Tiles[location].Color);
    }
}