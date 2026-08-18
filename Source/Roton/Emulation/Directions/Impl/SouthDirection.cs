using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

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