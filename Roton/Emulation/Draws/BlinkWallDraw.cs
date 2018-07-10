using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Draws
{
    public class BlinkWallDraw : IDraw
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