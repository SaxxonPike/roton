using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class WebBehavior : ElementBehavior
    {
        private readonly IElements _elements;
        private readonly IState _state;
        private readonly IGrid _grid;
        
        public override string KnownName => KnownNames.Web;

        public WebBehavior(IElements elements, IState state, IGrid grid)
        {
            _elements = elements;
            _state = state;
            _grid = grid;
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_state.WebChars[_grid.Adjacent(location, _elements.WebId)],
                _grid[location].Color);
        }
    }
}