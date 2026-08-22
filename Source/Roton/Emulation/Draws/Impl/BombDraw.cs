using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public sealed class BombDraw(
    ITiles tiles,
    IActorList actorList)
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        var p1 = actorList.ActorAt(location).P1;
        return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, tiles[location].Color);
    }
}