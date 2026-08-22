using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterDraw(IEngineAccessor engine) : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        var actor = Engine.ActorAt(location);

        var index = actor.Cycle > 0 
            ? (Engine.State.GameCycle / actor.Cycle) & 0x3 
            : 0;
                
        if (actor.Vector.X == 0)
        {
            index += (actor.Vector.Y << 1) + 2;
            return new AnsiChar(Engine.State.TransporterVChars[index], tiles[location].Color);
        }

        index += (actor.Vector.X << 1) + 2;
        return new AnsiChar(Engine.State.TransporterHChars[index], tiles[location].Color);
    }
}