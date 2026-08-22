using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x40)]
public sealed class StoneDraw(
    IEngineAccessor engine,
    ITiles tiles,
    IRandomizer randomizer)
    : IDraw
{
    private IEngine Engine => engine.Instance;

    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(0x41 + randomizer.GetNext(0x1A), tiles[location].Color);
    }
}