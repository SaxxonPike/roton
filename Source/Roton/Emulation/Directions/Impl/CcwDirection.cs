using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CCW")]
[Context(Context.Super, "CCW")]
public sealed class CcwDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Parser.GetDirection(context).CounterClockwise();
    }
}