using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, id: 0x1D)]
[Context(Context.Super, id: 0x1D)]
internal sealed class BlinkWallDraw(
    ITiles tiles)
    : IDraw
{
    /// <remarks>
    /// RoZ: ElementBlinkWallDraw
    /// </remarks>
    public AnsiChar Draw(Location location) => 
        new(0xCE, tiles[location].Color);
}