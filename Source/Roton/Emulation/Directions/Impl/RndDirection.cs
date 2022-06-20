using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RND")]
[Context(Context.Super, "RND")]
public sealed class RndDirection : IDirection
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public RndDirection(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Rnd();
    }
}