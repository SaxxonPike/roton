using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BoardEdgeBehavior : ElementBehavior
    {
        private readonly IWorld _world;
        private readonly IGrid _grid;
        private readonly IBoard _board;
        private readonly IElements _elements;
        private readonly IEngine _engine;
        private readonly IState _state;

        public override string KnownName => KnownNames.BoardEdge;

        public BoardEdgeBehavior(IWorld world, IGrid grid, IBoard board, IElements elements, IEngine engine, IState state)
        {
            _world = world;
            _grid = grid;
            _board = board;
            _elements = elements;
            _engine = engine;
            _state = state;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var target = location.Clone();
            int targetBoard;
            var oldBoard = _world.BoardIndex;

            switch (vector.Y)
            {
                case -1:
                    targetBoard = _board.ExitNorth;
                    target.Y = _grid.Height;
                    break;
                case 1:
                    targetBoard = _board.ExitSouth;
                    target.Y = 1;
                    break;
                default:
                    if (vector.X == -1)
                    {
                        targetBoard = _board.ExitWest;
                        target.X = _grid.Width;
                    }
                    else
                    {
                        targetBoard = _board.ExitEast;
                        target.X = 1;
                    }
                    break;
            }

            if (targetBoard != 0)
            {
                _engine.SetBoard(targetBoard);
                if (_grid[target].Id != _elements.PlayerId)
                {
                    _grid.ElementAt(target).Interact(target, index, _state.KeyVector);
                }
                if (_grid.ElementAt(target).IsFloor || _grid.ElementAt(target).Id == _elements.PlayerId)
                {
                    if (_grid.ElementAt(target).Id != _elements.PlayerId)
                    {
                        __engine.MoveActor(0, target);
                    }
                    _engine.FadePurple();
                    vector.SetTo(0, 0);
                    _engine.EnterBoard();
                }
                else
                {
                    _engine.SetBoard(oldBoard);
                }
            }
        }
    }
}