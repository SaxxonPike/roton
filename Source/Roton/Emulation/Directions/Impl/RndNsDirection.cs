using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDNS")]
[Context(Context.Super, "RNDNS")]
public sealed class RndNsDirection(Lazy<IEngine> engine) : IDirection
{
    private IEngine Engine => engine.Value;

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Random.GetNext(2) == 0
            ? Vector.North
            : Vector.South;
    }
}