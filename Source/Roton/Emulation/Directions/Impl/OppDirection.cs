using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "OPP")]
[Context(Context.Super, "OPP")]
internal sealed class OppDirection(
    IDirectionEvaluator directionEvaluator)
    : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) => 
        directionEvaluator.TryEval(ref context, ref instruction, out var vec)
            ? -vec
            : Vector.Idle;
}