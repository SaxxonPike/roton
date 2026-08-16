using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CW")]
[Context(Context.Super, "CW")]
public sealed class CwDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public IXyPair Execute(IOopContext context)
    {
        return Engine.Parser.GetDirection(context).Clockwise();
    }
}