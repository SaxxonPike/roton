using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterDraw(
    IEngineAccessor engine,
    IActorList actorList,
    ITiles tiles,
    IState state)
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        var actor = actorList.ActorAt(location);

        var index = actor.Cycle > 0 
            ? (state.GameCycle / actor.Cycle) & 0x3 
            : 0;
                
        if (actor.Vector.X == 0)
        {
            index += (actor.Vector.Y << 1) + 2;
            return new AnsiChar(state.TransporterVChars[index], tiles[location].Color);
        }

        index += (actor.Vector.X << 1) + 2;
        return new AnsiChar(state.TransporterHChars[index], tiles[location].Color);
    }
}