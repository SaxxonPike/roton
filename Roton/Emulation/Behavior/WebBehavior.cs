using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class WebBehavior : ElementBehavior
    {
        public override string KnownName => "Web";

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(engine.StateData.WebChars[engine.Adjacent(location, engine.Elements.WebId)], engine.Tiles[location].Color);
        }
    }
}
