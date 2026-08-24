using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
public sealed class ObjectDraw(
    IActorList actorList,
    ITiles tiles) 
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(actorList.ActorAt(location).P1, tiles[location].Color);
    }
}