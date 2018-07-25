using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "W")]
    [ContextEngine(ContextEngine.Original, "WEST")]
    [ContextEngine(ContextEngine.Super, "W")]
    [ContextEngine(ContextEngine.Super, "WEST")]
    public sealed class WestDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.West;
        }
    }
}