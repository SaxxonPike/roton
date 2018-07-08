using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Directions
{
    public class RndNsDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndNsDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "RNDNS";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Random.Synced(2) == 0
                ? Vector.North
                : Vector.South;
        }
    }
}