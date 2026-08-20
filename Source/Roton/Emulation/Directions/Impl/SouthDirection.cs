using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "S")]
[Context(Context.Original, "SOUTH")]
[Context(Context.Super, "S")]
[Context(Context.Super, "SOUTH")]
public sealed class SouthDirection : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Vector.South;
    }
}