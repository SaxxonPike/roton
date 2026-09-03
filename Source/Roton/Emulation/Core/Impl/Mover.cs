using System;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Mover(
    IActorList actors,
    ITiles tiles,
    IElementList elements,
    IBoardUpdater boardUpdater,
    IBoard board,
    IHud hud,
    IInteractionList interactions,
    IFacts facts,
    IPlayerUpdater playerUpdater,
    ICamera camera,
    IWorld world)
    : IMover
{
    private ITiles _tiles = tiles;

    private int Distance(Location a, Location b) =>
        (a.Y - b.Y).Square() * facts.DistanceMultY + (a.X - b.X).Square();

    public void Move(int index, Location target)
    {
        var actor = actors[index];
        var sourceLocation = actor.Location;
        ref var sourceTile = ref _tiles[actor.Location];
        ref var targetTile = ref _tiles[target];
        var underTile = actor.UnderTile;
        var nextUnderTile = targetTile;

        var color = targetTile.Id == elements.EmptyId
            ? sourceTile.Color & 0x0F
            : (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F);

        targetTile = new Tile(sourceTile.Id, color);

        sourceTile = underTile;
        actor.Location = target;
        if (targetTile.Id == elements.PlayerId)
            playerUpdater.ForcePlayerColor(index);

        boardUpdater.UpdateBoard(target);
        boardUpdater.UpdateBoard(sourceLocation);
        actor.UnderTile = nextUnderTile;

        if (index == 0)
        {
            if (board.IsDark && world.TorchCycles > 0)
            {
                var squareDistanceX = (target.X - sourceLocation.X).Square();
                var squareDistanceY = (target.Y - sourceLocation.Y).Square();

                if (squareDistanceX + squareDistanceY == 1)
                {
                    // If the player is only moving one tile and entering a board that is unlit but has
                    // torch cycles active, only update tiles that have either gained or lost visibility.

                    var radX = facts.RadiusBoundX + 2;
                    var radY = facts.RadiusBoundY + 2;
                    var minX = Math.Max(target.X - radX, 1);
                    var maxX = Math.Min(target.X + radX, _tiles.Width);
                    var minY = Math.Max(target.Y - radY, 1);
                    var maxY = Math.Min(target.Y + radY, _tiles.Height);

                    for (var x = minX; x <= maxX; x++)
                    for (var y = minY; y <= maxY; y++)
                    {
                        var glowLocation = new Location(x, y);
                        if ((Distance(sourceLocation, glowLocation) < facts.TorchRadius) ^
                            (Distance(target, glowLocation) < facts.TorchRadius))
                            boardUpdater.UpdateBoard(glowLocation);
                    }
                }
            }

            if (camera.UpdateCamera())
                hud.RedrawBoard();
        }
    }

    public void Float(int index)
    {
        var actor = actors[index];
        var vector = new Vector();
        var underId = actor.UnderTile.Id;

        if (underId == elements.RiverNId)
            vector = Vector.North;
        else if (underId == elements.RiverSId)
            vector = Vector.South;
        else if (underId == elements.RiverWId)
            vector = Vector.West;
        else if (underId == elements.RiverEId)
            vector = Vector.East;

        if (vector.IsNonZero())
        {
            ref var actorTile = ref _tiles[actor.Location];
            if (actorTile.Id == elements.PlayerId)
            {
                var targetLocation = actor.Location + vector;
                interactions.Get(_tiles[targetLocation].Id)?.Interact(targetLocation, 0, ref vector);
            }
        }

        if (vector.IsNonZero())
        {
            var target = actor.Location + vector;
            if (_tiles.ElementAt(target).IsFloor)
                Move(index, target);
        }
    }
}