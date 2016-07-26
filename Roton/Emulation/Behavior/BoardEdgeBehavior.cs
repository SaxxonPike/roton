using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class BoardEdgeBehavior : ElementBehavior
    {
        public override string KnownName => "Board Edge";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var target = location.Clone();
            int targetBoard;
            var oldBoard = engine.World.BoardIndex;

            switch (vector.Y)
            {
                case -1:
                    targetBoard = engine.Board.ExitNorth;
                    target.Y = engine.Tiles.Height;
                    break;
                case 1:
                    targetBoard = engine.Board.ExitSouth;
                    target.Y = 1;
                    break;
                default:
                    if (vector.X == -1)
                    {
                        targetBoard = engine.Board.ExitWest;
                        target.X = engine.Tiles.Width;
                    }
                    else
                    {
                        targetBoard = engine.Board.ExitEast;
                        target.X = 1;
                    }
                    break;
            }

            if (targetBoard != 0)
            {
                engine.SetBoard(targetBoard);
                if (engine.TileAt(target).Id != engine.Elements.PlayerId)
                {
                    engine.ElementAt(target).Interact(engine, target, index, engine.State.KeyVector);
                }
                if (engine.ElementAt(target).IsFloor || engine.ElementAt(target).Id == engine.Elements.PlayerId)
                {
                    if (engine.ElementAt(target).Id != engine.Elements.PlayerId)
                    {
                        engine.MoveActor(0, target);
                    }
                    engine.FadePurple();
                    vector.SetTo(0, 0);
                    engine.EnterBoard();
                }
                else
                {
                    engine.SetBoard(oldBoard);
                }
            }
        }
    }
}