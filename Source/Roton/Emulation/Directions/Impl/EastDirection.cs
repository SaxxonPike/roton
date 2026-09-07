using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "E")]
[Context(Context.Original, "EAST")]
[Context(Context.Super, "E")]
[Context(Context.Super, "EAST")]
internal sealed class EastDirection : IDirection
{
    public Vector TryEval(ref OopContext context, ref Word instruction)
    {
        return Vector.East;
    }
}