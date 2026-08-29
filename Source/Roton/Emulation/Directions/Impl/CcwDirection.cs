using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CCW")]
[Context(Context.Super, "CCW")]
internal sealed class CcwDirection(
    IParser parser)
    : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) =>
        parser.TryEvalDirection(ref context, ref instruction, out var vec)
            ? vec.CounterClockwise()
            : Vector.Idle;
}