using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl
{
    [Context(Context.Original, "W")]
    [Context(Context.Original, "WEST")]
    [Context(Context.Super, "W")]
    [Context(Context.Super, "WEST")]
    public sealed class WestDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.West;
        }
    }
}