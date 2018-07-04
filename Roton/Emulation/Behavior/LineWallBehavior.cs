using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class LineWallBehavior : ElementBehavior
    {
        private readonly IState _state;
        private readonly IElements _elements;
        private readonly IGrid _grid;
        
        public override string KnownName => KnownNames.Line;

        public LineWallBehavior(IState state, IElements elements, IGrid grid)
        {
            _state = state;
            _elements = elements;
            _grid = grid;
        }
        
        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_state.LineChars[_grid.Adjacent(location, _elements.LineId)],
                _grid[location].Color);
        }
    }
}