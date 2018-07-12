using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Original, 0x0F)]
    [ContextEngine(ContextEngine.Super, 0x48)]
    public sealed class StarDraw : IDraw
    {
        private readonly IEngine _engine;

        public StarDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            var tile = _engine.Tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(_engine.State.StarChars[_engine.State.GameCycle & 0x3], tile.Color);
        }
    }
}