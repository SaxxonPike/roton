using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class NorthDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.North;
        }
    }
}