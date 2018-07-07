using Roton.Core;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class BoardEdgeBehavior : ElementBehavior
    {
        private readonly IWorld _world;
        private readonly ITiles _tiles;
        private readonly IBoard _board;
        private readonly IElements _elements;
        private readonly IEngine _engine;
        private readonly IState _state;
        private readonly IMover _mover;
        private readonly IMisc _misc;

        public override string KnownName => KnownNames.BoardEdge;

        public BoardEdgeBehavior(IWorld world, ITiles tiles, IBoard board, IElements elements, IEngine engine, IState state, IMover mover, IMisc misc)
        {
            _world = world;
            _tiles = tiles;
            _board = board;
            _elements = elements;
            _engine = engine;
            _state = state;
            _mover = mover;
            _misc = misc;
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
                    target.Y = _tiles.Height;
                    break;
                case 1:
                    targetBoard = _board.ExitSouth;
                    target.Y = 1;
                    break;
                default:
                    if (vector.X == -1)
                    {
                        targetBoard = _board.ExitWest;
                        target.X = _tiles.Width;
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
                if (_tiles[target].Id != _elements.PlayerId)
                {
                    _tiles.ElementAt(target).Interact(target, index, _state.KeyVector);
                }
                if (_tiles.ElementAt(target).IsFloor || _tiles.ElementAt(target).Id == _elements.PlayerId)
                {
                    if (_tiles.ElementAt(target).Id != _elements.PlayerId)
                    {
                        _mover.MoveActor(0, target);
                    }
                    _engine.FadePurple();
                    vector.SetTo(0, 0);
                    _misc.EnterBoard();
                }
                else
                {
                    _engine.SetBoard(oldBoard);
                }
            }
        }
    }
}