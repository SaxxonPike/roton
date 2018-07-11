using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class EastDirection : IDirection
    {
        public string Name => "EAST";
        
        public IXyPair Execute(IOopContext context)
        {
            return Vector.East;
        }
    }
}