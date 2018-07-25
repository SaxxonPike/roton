using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original, 0x01)]
    [ContextEngine(ContextEngine.Super, 0x01)]
    public sealed class BoardEdgeInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public BoardEdgeInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
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

            if (targetBoard == 0) 
                return;
            
            _engine.SetBoard(targetBoard);
            if (_engine.Tiles[target].Id != _engine.ElementList.PlayerId)
            {
                _engine.InteractionList.Get(_engine.Tiles[target].Id)
                    .Interact(target, index, _engine.State.KeyVector);
            }
            if (_engine.Tiles.ElementAt(target).IsFloor || _engine.Tiles.ElementAt(target).Id == _engine.ElementList.PlayerId)
            {
                if (_engine.Tiles.ElementAt(target).Id != _engine.ElementList.PlayerId)
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