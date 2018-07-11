using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class OppDirection : IDirection
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