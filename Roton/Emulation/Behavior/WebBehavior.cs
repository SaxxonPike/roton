using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class WebBehavior : ElementBehavior
    {
        public override string KnownName => "Web";

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_state.WebChars[engine.Adjacent(location, _elements.WebId)],
                _grid[location].Color);
        }
    }
}