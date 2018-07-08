using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Extensions;

namespace Roton.Emulation.Directions
{
    public class CwDirection : IDirection
    {
        private readonly IEngine _engine;

        public CwDirection(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "CW";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Parser.GetDirection(context).Clockwise();
        }
    }
}