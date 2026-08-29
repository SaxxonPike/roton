using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
internal sealed class SpinningGunDraw(
    ITiles tiles,
    IState state)
    : IDraw
{
    public AnsiChar Draw(Location location)
    {
        switch (state.GameCycle & 0x7)
        {
            case 0:
            case 1:
                return new AnsiChar(0x18, tiles[location].Color);
            case 2:
            case 3:
                return new AnsiChar(0x1A, tiles[location].Color);
            case 4:
            case 5:
                return new AnsiChar(0x19, tiles[location].Color);
            default:
                return new AnsiChar(0x1B, tiles[location].Color);
        }
    }
}