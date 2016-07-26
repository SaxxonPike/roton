using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class LineWallBehavior : ElementBehavior
    {
        public override string KnownName => "Line Wall";

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(engine.State.LineChars[engine.Adjacent(location, engine.Elements.LineId)], engine.Tiles[location].Color);
        }
    }
}
