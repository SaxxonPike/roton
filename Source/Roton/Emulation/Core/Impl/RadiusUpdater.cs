using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class RadiusUpdater(
    ITiles tiles,
    IFacts facts,
    IActorList actors,
    IBroadcaster broadcaster,
    IElementList elements,
    IRandomizer randomizer,
    IBoardUpdater boardUpdater,
    IDestroyer destroyer
) : IRadiusUpdater
{
    private static int Distance(Location a, Location b) =>
        (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();

    public void UpdateRadius(Location location, RadiusMode mode)
    {
        var source = location;
        var left = source.X - 9;
        var right = source.X + 9;
        var top = source.Y - 6;
        var bottom = source.Y + 6;
        for (var x = left; x <= right; x++)
        for (var y = top; y <= bottom; y++)
            if (x >= 1 && x <= tiles.Width && y >= 1 && y <= tiles.Height)
            {
                var target = new Location(x, y);
                if (mode != RadiusMode.Update)
                    if (Distance(source, target) < facts.TorchRadius)
                    {
                        var element = tiles.ElementAt(target);
                        if (mode == RadiusMode.Explode)
                        {
                            if (element.CanContainCode)
                            {
                                var actorIndex = actors.ActorIndexAt(target);
                                if (actorIndex > 0)
                                    broadcaster.BroadcastLabel(-actorIndex, facts.BombedLabel, false);
                            }

                            if (element.IsDestructible || element.Id == elements.StarId)
                                destroyer.Destroy(target);

                            if (element.Id == elements.EmptyId || element.Id == elements.BreakableId)
                                tiles[target] = new Tile(elements.BreakableId, randomizer.GetNext(7) + 9);
                        }
                        else
                        {
                            if (tiles[target].Id == elements.BreakableId)
                                tiles[target].Id = elements.EmptyId;
                        }
                    }

                boardUpdater.UpdateBoard(target);
            }
    }
}