using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Extensions;

namespace Roton.Emulation.Directions
{
    public class CcwDirection : IDirection
    {
        private readonly IEngine _engine;

        public CcwDirection(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "CCW";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Parser.GetDirection(context).CounterClockwise();
        }
    }
}