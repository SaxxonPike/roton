using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CCW")]
[Context(Context.Super, "CCW")]
internal sealed class CcwDirection(
    IDirectionEvaluator directionEvaluator)
    : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) =>
        directionEvaluator.TryEval(ref context, ref instruction, out var vec)
            ? vec.CounterClockwise()
            : Vector.Idle;
}