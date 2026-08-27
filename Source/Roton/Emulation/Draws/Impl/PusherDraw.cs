using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
public sealed class PusherDraw(
    IActorList actorList,
    ITiles tiles) 
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        var actor = actorList.ActorAt(location);
        return actor.Vector.X switch
        {
            1 => new AnsiChar(0x10, tiles[location].Color),
            -1 => new AnsiChar(0x11, tiles[location].Color),
            _ => actor.Vector.Y == -1
                ? new AnsiChar(0x1E, tiles[location].Color)
                : new AnsiChar(0x1F, tiles[location].Color)
        };
    }
}