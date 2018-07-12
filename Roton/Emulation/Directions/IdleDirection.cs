using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "I")]
    [ContextEngine(ContextEngine.Original, "IDLE")]
    [ContextEngine(ContextEngine.Super, "I")]
    [ContextEngine(ContextEngine.Super, "IDLE")]
    public sealed class IdleDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.Idle;
        }
    }
}