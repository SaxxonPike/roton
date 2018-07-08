using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Directions
{
    public class EastDirection : IDirection
    {
        public string Name => "EAST";
        
        public IXyPair Execute(IOopContext context)
        {
            return Vector.East;
        }
    }
}