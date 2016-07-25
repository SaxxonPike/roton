using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class LineWallBehavior : ElementBehavior
    {
        public override string KnownName => "Line Wall";

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(engine.StateData.LineChars[engine.Adjacent(location, engine.Elements.LineId)], engine.Tiles[location].Color);
        }
    }
}
