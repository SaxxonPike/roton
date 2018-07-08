using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Directions
{
    public class RndNeDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndNeDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "RNDNE";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Random.Synced(2) == 0
                ? Vector.North
                : Vector.East;
        }
    }
}