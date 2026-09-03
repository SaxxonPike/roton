using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CW")]
[Context(Context.Super, "CW")]
internal sealed class CwDirection(
    IDirectionEvaluator directionEvaluator) : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) => 
        directionEvaluator.TryEval(ref context, ref instruction, out var vec)
            ? vec.Clockwise()
            : Vector.Idle;
}