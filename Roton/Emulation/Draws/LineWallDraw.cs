using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Draws;

namespace Roton.Emulation.Behaviors
{
    public class LineWallDraw : IDraw
    {
        private readonly IEngine _engine;

        public LineWallDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_engine.State.LineChars[_engine.Adjacent(location, _engine.ElementList.LineId)],
                _engine.Tiles[location].Color);
        }
    }
}