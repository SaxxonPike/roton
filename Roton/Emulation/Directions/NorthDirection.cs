using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Directions
{
    public class NorthDirection : IDirection
    {
        public string Name => "NORTH";
        
        public IXyPair Execute(IOopContext context)
        {
            return Vector.North;
        }
    }
}