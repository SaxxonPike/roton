using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class WebBehavior : ElementBehavior
    {
        private readonly IElements _elements;
        private readonly IState _state;
        private readonly ITiles _tiles;
        
        public override string KnownName => KnownNames.Web;

        public WebBehavior(IElements elements, IState state, ITiles tiles)
        {
            _elements = elements;
            _state = state;
            _tiles = tiles;
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_state.WebChars[_tiles.Adjacent(location, _elements.WebId)],
                _tiles[location].Color);
        }
    }
}