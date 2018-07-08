using Roton.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Directions
{
    public class FlowDirection : IDirection
    {
        public string Name => "FLOW";
        
        public IXyPair Execute(IOopContext context)
        {
            return context.Actor.Vector.Clone();
        }
    }
}