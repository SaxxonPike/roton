using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1D)]
[Context(Context.Super, 0x1D)]
public sealed class BlinkWallDraw(
    ITiles tiles)
    : IDraw
{
    public AnsiChar Draw(Location location) => 
        new(0xCE, tiles[location].Color);
}