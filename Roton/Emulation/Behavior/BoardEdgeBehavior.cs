using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BoardEdgeBehavior : ElementBehavior
    {
        private readonly IWorld _world;
        private readonly ITileGrid _tileGrid;
        private readonly IBoard _board;
        private readonly IElementList _elementList;
        private readonly IEngine _engine;
        private readonly IState _state;

        public override string KnownName => KnownNames.BoardEdge;

        public BoardEdgeBehavior(IWorld world, ITileGrid tileGrid, IBoard board, IElementList elementList, IEngine engine, IState state)
        {
            _world = world;
            _tileGrid = tileGrid;
            _board = board;
            _elementList = elementList;
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
                    target.Y = _tileGrid.Height;
                    break;
                case 1:
                    targetBoard = _board.ExitSouth;
                    target.Y = 1;
                    break;
                default:
                    if (vector.X == -1)
                    {
                        targetBoard = _board.ExitWest;
                        target.X = _tileGrid.Width;
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
                if (_engine.TileAt(target).Id != _elementList.PlayerId)
                {
                    _engine.ElementAt(target).Interact(_engine, target, index, _state.KeyVector);
                }
                if (_engine.ElementAt(target).IsFloor || _engine.ElementAt(target).Id == _elementList.PlayerId)
                {
                    if (_engine.ElementAt(target).Id != _elementList.PlayerId)
                    {
                        _engine.MoveActor(0, target);
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