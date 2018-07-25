using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "N")]
    [ContextEngine(ContextEngine.Original, "NORTH")]
    [ContextEngine(ContextEngine.Super, "N")]
    [ContextEngine(ContextEngine.Super, "NORTH")]
    public sealed class NorthDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.North;
        }
    }
}