using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Draws;

namespace Roton.Emulation.Behaviors
{
    public class BombDraw : IDraw
    {
        private readonly IEngine _engine;

        public BombDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            var p1 = _engine.ActorAt(location).P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, _engine.Tiles[location].Color);
        }
    }
}