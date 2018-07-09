using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Draws;

namespace Roton.Emulation.Behaviors
{
    public class StoneDraw : IDraw
    {
        private readonly IEngine _engine;

        public StoneDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x41 + _engine.Random.NonSynced(0x1A), _engine.Tiles[location].Color);
        }
    }
}