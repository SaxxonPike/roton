using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
internal sealed class TransporterDraw(
    IActorList actorList,
    ITiles tiles,
    IState state)
    : IDraw
{
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