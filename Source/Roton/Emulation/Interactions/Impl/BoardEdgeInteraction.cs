using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x01)]
[Context(Context.Super, 0x01)]
public sealed class BoardEdgeInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var target = location;
        int targetBoard;
        var oldBoard = Engine.World.BoardIndex;

        switch (vector.X, vector.Y)
        {
            case (_, -1):
                targetBoard = Engine.Board.Exits.North;
                target.Y = Engine.Tiles.Height;
                break;
            case (_, 1):
                targetBoard = Engine.Board.Exits.South;
                target.Y = 1;
                break;
            case (-1, _):
                targetBoard = Engine.Board.Exits.West;
                target.X = Engine.Tiles.Width;
                break;
            default:
                targetBoard = Engine.Board.Exits.East;
                target.X = 1;
                break;
        }

        if (targetBoard == 0)
            return;

        Engine.SetBoard(targetBoard);
        if (Engine.Tiles[target].Id != Engine.ElementList.PlayerId)
        {
            Engine.InteractionList.Get(Engine.Tiles[target].Id)
                .Interact(target, index, ref Engine.State.KeyVector);
        }

        if (Engine.Tiles.ElementAt(target).IsFloor ||
            Engine.Tiles.ElementAt(target).Id == Engine.ElementList.PlayerId)
        {
            if (Engine.Tiles.ElementAt(target).Id != Engine.ElementList.PlayerId)
            {
                Engine.MoveActor(0, target);
            }

            Engine.FadePurple();
            vector = Vector.Idle;
            Engine.EnterBoard();
        }
        else
        {
            Engine.SetBoard(oldBoard);
        }
    }
}