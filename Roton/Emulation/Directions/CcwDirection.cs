using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "CCW")]
    [ContextEngine(ContextEngine.Super, "CCW")]
    public sealed class CcwDirection : IDirection
    {
        private readonly IEngine _engine;

        public CcwDirection(IEngine engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            return _engine.Parser.GetDirection(context).CounterClockwise();
        }
    }
}