using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "OPP")]
[Context(Context.Super, "OPP")]
public sealed class OppDirection : IDirection
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public OppDirection(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Parser.GetDirection(context).Opposite();
    }
}