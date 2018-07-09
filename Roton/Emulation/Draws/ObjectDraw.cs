using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Draws;

namespace Roton.Emulation.Behaviors
{
    public class ObjectDraw : IDraw
    {
        private readonly IEngine _engine;

        public ObjectDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_engine.ActorAt(location).P1, _engine.Tiles[location].Color);
        }
    }
}