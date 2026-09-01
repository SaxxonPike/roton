using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x01)]
[Context(Context.Super, 0x01)]
internal sealed class BoardEdgeInteraction(
    IEngineAccessor engine,
    IWorld world,
    ITiles tiles,
    IElementList elementList,
    IInteractionList interactionList,
    IState state,
    IWorldUnit worldUnit,
    IFeatures features,
    IMover mover,
    IExits exits)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

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

        worldUnit.SetBoard(targetBoard);
        if (tiles[target].Id != elementList.PlayerId)
        {
            interactionList.Get(tiles[target].Id)?
                .Interact(target, index, ref state.KeyVector);
        }

        if (tiles.ElementAt(target).IsFloor ||
            tiles.ElementAt(target).Id == elementList.PlayerId)
        {
            if (tiles.ElementAt(target).Id != elementList.PlayerId)
            {
                mover.MoveActor(0, target);
            }

            Engine.FadePurple();
            vector = Vector.Idle;
            features.EnterBoard();
        }
        else
        {
            worldUnit.SetBoard(oldBoard);
        }
    }
}