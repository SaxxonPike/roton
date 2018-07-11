using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Zzt, 0x1D)]
    [ContextEngine(ContextEngine.SuperZzt, 0x1D)]
    public sealed class BlinkWallDraw : IDraw
    {
        private readonly IEngine _engine;

        public BlinkWallDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0xCE, _engine.Tiles[location].Color);
        }
    }
}