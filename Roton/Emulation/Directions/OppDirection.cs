using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "OPP")]
    [ContextEngine(ContextEngine.Super, "OPP")]
    public sealed class OppDirection : IDirection
    {
        private readonly IEngine _engine;

        public OppDirection(IEngine engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            return _engine.Parser.GetDirection(context).Opposite();
        }
    }
}