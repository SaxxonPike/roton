using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1D)]
[Context(Context.Super, 0x1D)]
public sealed class BlinkWallDraw(Lazy<IEngine> engine) : IDraw
{
    private IEngine Engine => engine.Value;

    public AnsiChar Draw(IXyPair location)
    {
        return new AnsiChar(0xCE, Engine.Tiles[location].Color);
    }
}