using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Directions
{
    public class RndPDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndPDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "RNDP";
        
        public IXyPair Execute(IOopContext context)
        {
            var direction = _engine.Parser.GetDirection(context);
            return _engine.Random.Synced(2) == 0
                ? direction.Clockwise()
                : direction.CounterClockwise();
        }
    }
}