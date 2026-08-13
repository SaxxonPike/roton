using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CW")]
[Context(Context.Super, "CW")]
public sealed class CwDirection : IDirection
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public CwDirection(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Parser.GetDirection(context).Clockwise();
    }
}