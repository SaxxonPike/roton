using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "E")]
    [ContextEngine(ContextEngine.Original, "EAST")]
    [ContextEngine(ContextEngine.Super, "E")]
    [ContextEngine(ContextEngine.Super, "EAST")]
    public sealed class EastDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.East;
        }
    }
}