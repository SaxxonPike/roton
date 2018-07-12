using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "RNDNE")]
    [ContextEngine(ContextEngine.Super, "RNDNE")]
    public sealed class RndNeDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndNeDirection(IEngine engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            return _engine.Random.Synced(2) == 0
                ? Vector.North
                : Vector.East;
        }
    }
}