using Roton.Core;

namespace Roton.Emulation.Directions
{
    public class WestDirection : IDirection
    {
        public string Name => "WEST";
        
        public IXyPair Execute(IOopContext context)
        {
            return Vector.West;
        }
    }
}