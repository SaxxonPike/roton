using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class WebBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Web;

        public WebBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_engine.State.WebChars[_engine.Adjacent(location, _engine.Elements.WebId)],
                _engine.Tiles[location].Color);
        }
    }
}