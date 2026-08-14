using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Super, 0x3C)]
public sealed class DragonPupDraw(Lazy<IEngine> engine) : IDraw
{
    private IEngine Engine => engine.Value;

    public AnsiChar Draw(IXyPair location)
    {
        switch (Engine.State.GameCycle & 0x3)
        {
            case 0:
            case 2:
                return new AnsiChar(0x94, Engine.Tiles[location].Color);
            case 1:
                return new AnsiChar(0xA2, Engine.Tiles[location].Color);
            default:
                return new AnsiChar(0x95, Engine.Tiles[location].Color);
        }
    }
}