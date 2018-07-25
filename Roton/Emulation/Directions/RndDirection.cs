using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original, "RND")]
    [ContextEngine(ContextEngine.Super, "RND")]
    public sealed class RndDirection : IDirection
    {
        private readonly IEngine _engine;

        public RndDirection(IEngine engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            return _engine.Rnd();
        }
    }
}