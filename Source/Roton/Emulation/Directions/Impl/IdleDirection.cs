using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "I")]
[Context(Context.Original, "IDLE")]
[Context(Context.Super, "I")]
[Context(Context.Super, "IDLE")]
public sealed class IdleDirection : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Vector.Idle;
    }
}