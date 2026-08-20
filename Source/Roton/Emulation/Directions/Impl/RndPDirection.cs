using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDP")]
[Context(Context.Super, "RNDP")]
public sealed class RndPDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction) =>
        Engine.Parser.TryEvalDirection(ref context, ref instruction, out var direction)
            ? Engine.Random.GetNext(2) == 0
                ? direction.Clockwise()
                : direction.CounterClockwise()
            : Vector.Idle;
}