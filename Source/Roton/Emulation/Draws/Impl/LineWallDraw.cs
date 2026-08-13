using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original, 0x1F)]
[Context(Context.Super, 0x1F)]
public sealed class LineWallDraw(Lazy<IEngine> engine) : IDraw
{
    private IEngine Engine => engine.Value;

    public AnsiChar Draw(IXyPair location)
    {
        return new AnsiChar(Engine.State.LineChars[Engine.Adjacent(location, Engine.ElementList.LineId)],
            Engine.Tiles[location].Color);
    }
}