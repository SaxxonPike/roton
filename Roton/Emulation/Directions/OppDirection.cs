using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Directions
{
    public class OppDirection : IDirection
    {
        private readonly IEngine _engine;

        public OppDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "OPP";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Parser.GetDirection(context).Opposite();
        }
    }
}