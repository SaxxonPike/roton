using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDP")]
[Context(Context.Super, "RNDP")]
public sealed class RndPDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public IXyPair Execute(IOopContext context)
    {
        var direction = Engine.Parser.GetDirection(context);
        return Engine.Random.GetNext(2) == 0
            ? direction.Clockwise()
            : direction.CounterClockwise();
    }
}