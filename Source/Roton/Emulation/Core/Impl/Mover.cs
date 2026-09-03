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
    ICamera camera)
    : IMover
{
    private ITiles _tiles = tiles;

    private static int Distance(Location a, Location b) =>
        (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();

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
            if (board.IsDark)
            {
                var squareDistanceX = (target.X - sourceLocation.X).Square();
                var squareDistanceY = (target.Y - sourceLocation.Y).Square();
                if (squareDistanceX + squareDistanceY == 1)
                {
                    for (var x = target.X - facts.TorchDrawBoxVerticalSize;
                         x <= target.X + facts.TorchDrawBoxVerticalSize;
                         x++)
                    for (var y = target.Y - facts.TorchDrawBoxHorizontalSize;
                         y <= target.Y + facts.TorchDrawBoxHorizontalSize;
                         y++)
                    {
                        var glowLocation = new Location(x, y);
                        if (glowLocation.X >= 1 && glowLocation.X <= _tiles.Width && glowLocation.Y >= 1 &&
                            glowLocation.Y <= _tiles.Height)
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