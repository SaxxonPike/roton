using Roton.Core;

namespace Roton.Emulation.Directions
{
    public class SeekDirection : IDirection
    {
        private readonly IEngine _engine;

        public SeekDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "SEEK";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Seek(context.Actor.Location);
        }
    }
}