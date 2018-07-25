using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "RNDNS")]
    [ContextEngine(ContextEngine.Super, "RNDNS")]
    public sealed class RndNsDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndNsDirection(IEngine engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            return _engine.Random.Synced(2) == 0
                ? Vector.North
                : Vector.South;
        }
    }
}