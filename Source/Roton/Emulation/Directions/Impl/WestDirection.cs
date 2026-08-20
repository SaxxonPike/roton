using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "W")]
[Context(Context.Original, "WEST")]
[Context(Context.Super, "W")]
[Context(Context.Super, "WEST")]
public sealed class WestDirection : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Vector.West;
    }
}