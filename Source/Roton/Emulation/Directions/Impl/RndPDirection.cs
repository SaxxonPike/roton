using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDP")]
[Context(Context.Super, "RNDP")]
internal sealed class RndPDirection(
    IParser parser,
    IRandomizer randomizer)
    : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) =>
        parser.TryEvalDirection(ref context, ref instruction, out var direction)
            ? randomizer.GetNext(2) == 0
                ? direction.Clockwise()
                : direction.CounterClockwise()
            : Vector.Idle;
}