using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Conveyor(
    ITiles tiles,
    IElementList elements,
    IState state,
    IActorList actors,
    IBoardUpdater boardUpdater,
    IMover mover)
    : IConveyor
{
    private ITiles _tiles = tiles;
    private Vector GetConveyorVector(int index) => new(state.Vector8[index], state.Vector8[index + 8]);

    public void Convey(Location center, int direction)
    {
        int beginIndex;
        int endIndex;

        Span<Tile> surrounding = stackalloc Tile[8];

        if (direction == 1)
        {
            beginIndex = 0;
            endIndex = 8;
        }
        else
        {
            beginIndex = 7;
            endIndex = -1;
        }

        var pushable = true;
        for (var i = beginIndex; i != endIndex; i += direction)
        {
            surrounding[i] = _tiles[center + GetConveyorVector(i)];
            var element = elements[surrounding[i].Id];
            if (element.Id == elements.EmptyId)
                pushable = true;
            else if (!element.IsPushable)
                pushable = false;
        }

        for (var i = beginIndex; i != endIndex; i += direction)
        {
            var element = elements[surrounding[i].Id];

            if (pushable)
            {
                if (element.IsPushable)
                {
                    var source = center + GetConveyorVector(i);
                    var target = center + GetConveyorVector((i + 8 - direction) % 8);
                    if (element.Cycle > -1)
                    {
                        ref var tile = ref _tiles[source];
                        var index = actors.ActorIndexAt(source);
                        _tiles[source] = surrounding[i];
                        _tiles[target].Id = elements.EmptyId;
                        mover.Move(index, target);
                        _tiles[source] = tile;
                    }
                    else
                    {
                        _tiles[target] = surrounding[i];
                        boardUpdater.UpdateBoard(target);
                    }

                    if (!elements[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                    {
                        _tiles[source].Id = elements.EmptyId;
                        boardUpdater.UpdateBoard(source);
                    }
                }
                else
                {
                    pushable = false;
                }
            }
            else
            {
                if (element.Id == elements.EmptyId)
                    pushable = true;
            }
        }
    }
}