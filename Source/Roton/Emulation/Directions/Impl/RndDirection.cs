using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RND")]
[Context(Context.Super, "RND")]
public sealed class RndDirection(Lazy<IEngine> engine) : IDirection
{
    private IEngine Engine => engine.Value;

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Rnd();
    }
}