using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x01)]
[Context(Context.Super, 0x01)]
public sealed class BoardEdgeInteraction : IInteraction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public BoardEdgeInteraction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        var target = location.Clone();
        int targetBoard;
        var oldBoard = Engine.World.BoardIndex;

        switch (vector.Y)
        {
            case -1:
                targetBoard = Engine.Board.ExitNorth;
                target.Y = Engine.Tiles.Height;
                break;
            case 1:
                targetBoard = Engine.Board.ExitSouth;
                target.Y = 1;
                break;
            default:
                if (vector.X == -1)
                {
                    targetBoard = Engine.Board.ExitWest;
                    target.X = Engine.Tiles.Width;
                }
                else
                {
                    targetBoard = Engine.Board.ExitEast;
                    target.X = 1;
                }

                break;
        }

        if (targetBoard == 0)
            return;

        Engine.SetBoard(targetBoard);
        if (Engine.Tiles[target].Id != Engine.ElementList.PlayerId)
        {
            Engine.InteractionList.Get(Engine.Tiles[target].Id)
                .Interact(target, index, Engine.State.KeyVector);
        }

        if (Engine.Tiles.ElementAt(target).IsFloor ||
            Engine.Tiles.ElementAt(target).Id == Engine.ElementList.PlayerId)
        {
            if (Engine.Tiles.ElementAt(target).Id != Engine.ElementList.PlayerId)
            {
                Engine.MoveActor(0, target);
            }

            Engine.FadePurple();
            vector.SetTo(0, 0);
            Engine.EnterBoard();
        }
        else
        {
            Engine.SetBoard(oldBoard);
        }
    }
}