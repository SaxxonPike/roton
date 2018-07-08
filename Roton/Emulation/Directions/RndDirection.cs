using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Directions
{
    public class RndDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "RND";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Rnd();
        }
    }
}