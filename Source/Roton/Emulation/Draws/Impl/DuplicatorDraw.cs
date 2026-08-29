using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
internal sealed class DuplicatorDraw(
    IActorList actorList,
    ITiles tiles) 
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        return (int)actorList.ActorAt(location).P1 switch
        {
            2 => new AnsiChar(0xF9, tiles[location].Color),
            3 => new AnsiChar(0xF8, tiles[location].Color),
            4 => new AnsiChar(0x6F, tiles[location].Color),
            5 => new AnsiChar(0x4F, tiles[location].Color),
            _ => new AnsiChar(0xFA, tiles[location].Color)
        };
    }
}