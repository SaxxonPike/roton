using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "RNDP")]
    [ContextEngine(ContextEngine.Super, "RNDP")]
    public sealed class RndPDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndPDirection(IEngine engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            var direction = _engine.Parser.GetDirection(context);
            return _engine.Random.GetNext(2) == 0
                ? direction.Clockwise()
                : direction.CounterClockwise();
        }
    }
}