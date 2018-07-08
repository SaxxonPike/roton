using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Directions
{
    public class IdleDirection : IDirection
    {
        public string Name => "IDLE";
        
        public IXyPair Execute(IOopContext context)
        {
            return Vector.Idle;
        }
    }
}