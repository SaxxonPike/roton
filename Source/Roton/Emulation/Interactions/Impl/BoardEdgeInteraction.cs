using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x01)]
[Context(Context.Super, 0x01)]
internal sealed class BoardEdgeInteraction(
    IWorld world,
    ITiles tiles,
    IElementList elements,
    IInteractionList interactions,
    IState state,
    IWorldManager worldManager,
    IMover mover,
    IExits exits,
    IPlayerEnterHandler playerEnterHandler,
    IFader fader)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        var target = location;
        int targetBoard;
        var oldBoard = world.BoardIndex;

        switch (vector.X, vector.Y)
        {
            case (_, -1):
                targetBoard = exits.North;
                target.Y = tiles.Height;
                break;
            case (_, 1):
                targetBoard = exits.South;
                target.Y = 1;
                break;
            case (-1, _):
                targetBoard = exits.West;
                target.X = tiles.Width;
                break;
            default:
                targetBoard = exits.East;
                target.X = 1;
                break;
        }

        if (targetBoard == 0)
            return;

        worldManager.SetBoard(targetBoard);
        if (tiles[target].Id != elements.PlayerId)
        {
            interactions.Get(tiles[target].Id)?
                .Interact(target, index, ref state.KeyVector);
        }

        if (tiles.ElementAt(target).IsFloor ||
            tiles.ElementAt(target).Id == elements.PlayerId)
        {
            if (tiles.ElementAt(target).Id != elements.PlayerId)
            {
                mover.MoveActor(0, target);
            }

            fader.FadePurple();
            vector = Vector.Idle;
            playerEnterHandler.EnterBoard();
        }
        else
        {
            worldManager.SetBoard(oldBoard);
        }
    }
}