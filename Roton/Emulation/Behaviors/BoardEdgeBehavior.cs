using Roton.Core;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class BoardEdgeBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.BoardEdge;

        public BoardEdgeBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var target = location.Clone();
            int targetBoard;
            var oldBoard = _engine.World.BoardIndex;

            switch (vector.Y)
            {
                case -1:
                    targetBoard = _engine.Board.ExitNorth;
                    target.Y = _engine.Tiles.Height;
                    break;
                case 1:
                    targetBoard = _engine.Board.ExitSouth;
                    target.Y = 1;
                    break;
                default:
                    if (vector.X == -1)
                    {
                        targetBoard = _engine.Board.ExitWest;
                        target.X = _engine.Tiles.Width;
                    }
                    else
                    {
                        targetBoard = _engine.Board.ExitEast;
                        target.X = 1;
                    }
                    break;
            }

            if (targetBoard != 0)
            {
                _engine.SetBoard(targetBoard);
                if (_engine.Tiles[target].Id != _engine.Elements.PlayerId)
                {
                    _engine.Tiles.ElementAt(target).Interact(target, index, _engine.State.KeyVector);
                }
                if (_engine.Tiles.ElementAt(target).IsFloor || _engine.Tiles.ElementAt(target).Id == _engine.Elements.PlayerId)
                {
                    if (_engine.Tiles.ElementAt(target).Id != _engine.Elements.PlayerId)
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