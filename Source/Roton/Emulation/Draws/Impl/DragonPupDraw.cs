using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3C)]
internal sealed class DragonPupDraw(
    ITiles tiles,
    IState state)
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        switch (state.GameCycle & 0x3)
        {
            case 0:
            case 2:
                return new AnsiChar(0x94, tiles[location].Color);
            case 1:
                return new AnsiChar(0xA2, tiles[location].Color);
            default:
                return new AnsiChar(0x95, tiles[location].Color);
        }
    }
}