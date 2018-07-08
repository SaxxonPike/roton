using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Directions
{
    public class SouthDirection : IDirection
    {
        public string Name => "SOUTH";
        
        public IXyPair Execute(IOopContext context)
        {
            return Vector.South;
        }
    }
}