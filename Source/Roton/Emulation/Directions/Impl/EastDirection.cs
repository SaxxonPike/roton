using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl
{
    [Context(Context.Original, "E")]
    [Context(Context.Original, "EAST")]
    [Context(Context.Super, "E")]
    [Context(Context.Super, "EAST")]
    public sealed class EastDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.East;
        }
    }
}