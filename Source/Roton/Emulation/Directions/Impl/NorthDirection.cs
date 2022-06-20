using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "N")]
[Context(Context.Original, "NORTH")]
[Context(Context.Super, "N")]
[Context(Context.Super, "NORTH")]
public sealed class NorthDirection : IDirection
{
    public IXyPair Execute(IOopContext context)
    {
        return Vector.North;
    }
}