using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class WebBehavior : ElementBehavior
    {
        public override string KnownName => "Web";

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(engine.State.WebChars[engine.Adjacent(location, engine.Elements.WebId)],
                engine.Tiles[location].Color);
        }
    }
}