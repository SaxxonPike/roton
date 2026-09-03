using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
internal sealed class ObjectDraw(
    IActorList actors,
    ITiles tiles) 
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(actors.ActorAt(location).P1, tiles[location].Color);
    }
}