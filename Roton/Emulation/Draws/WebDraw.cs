using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Super, 0x3F)]
    public sealed class WebDraw : IDraw
    {
        private readonly IEngine _engine;
        
        public WebDraw(IEngine engine)
        {
            _engine = engine;
        }

        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_engine.State.WebChars[_engine.Adjacent(location, _engine.ElementList.WebId)],
                _engine.Tiles[location].Color);
        }
    }
}