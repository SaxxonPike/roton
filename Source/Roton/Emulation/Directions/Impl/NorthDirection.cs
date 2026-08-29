using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "N")]
[Context(Context.Original, "NORTH")]
[Context(Context.Super, "N")]
[Context(Context.Super, "NORTH")]
internal sealed class NorthDirection : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Vector.North;
    }
}