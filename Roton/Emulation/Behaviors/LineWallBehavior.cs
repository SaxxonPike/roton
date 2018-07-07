using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class LineWallBehavior : ElementBehavior
    {
        private readonly IState _state;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        
        public override string KnownName => KnownNames.Line;

        public LineWallBehavior(IState state, IElements elements, ITiles tiles)
        {
            _state = state;
            _elements = elements;
            _tiles = tiles;
        }
        
        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_state.LineChars[_tiles.Adjacent(location, _elements.LineId)],
                _tiles[location].Color);
        }
    }
}