using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "S")]
[Context(Context.Original, "SOUTH")]
[Context(Context.Super, "S")]
[Context(Context.Super, "SOUTH")]
public sealed class SouthDirection : IDirection
{
    public IXyPair Execute(IOopContext context)
    {
        return Vector.South;
    }
}