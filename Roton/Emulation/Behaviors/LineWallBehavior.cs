using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class LineWallBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Line;

        public LineWallBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_engine.State.LineChars[_engine.Adjacent(location, _engine.Elements.LineId)],
                _engine.Tiles[location].Color);
        }
    }
}