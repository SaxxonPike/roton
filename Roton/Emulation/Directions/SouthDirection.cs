using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "S")]
    [ContextEngine(ContextEngine.Original, "SOUTH")]
    [ContextEngine(ContextEngine.Super, "S")]
    [ContextEngine(ContextEngine.Super, "SOUTH")]
    public sealed class SouthDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.South;
        }
    }
}