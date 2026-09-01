using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Pusher(
    ITiles tiles,
    IElementList elements,
    IEngineAccessor engine,
    IActorList actors,
    ISoundUnit soundUnit,
    ISounds sounds,
    IBoardUpdater boardUpdater,
    ITracer tracer,
    IMover mover,
    ITileRemover tileRemover)
    : IPusher
{
    private IEngine Engine => engine.Instance;

    public void Push(Location location, Vector vector)
    {
        ref var tile = ref tiles[location];
        if (tile.Id == elements.SliderEwId && vector.Y == 0 ||
            tile.Id == elements.SliderNsId && vector.X == 0 ||
            elements[tile.Id].IsPushable)
        {
            if (vector.IsZero())
            {
                // This would ordinarily cause an infinite loop.
                tracer.TraceCrash("Push called with zero vector");
                return;
            }

            ref var furtherTile = ref tiles[location + vector];
            if (furtherTile.Id == elements.TransporterId)
                Transport(location, vector);
            else if (furtherTile.Id != elements.EmptyId)
                Push(location + vector, vector);

            var furtherElement = elements[furtherTile.Id];
            if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != elements.PlayerId)
                Engine.Destroy(location + vector);

            furtherElement = elements[furtherTile.Id];
            if (furtherElement.IsFloor)
                MoveTile(location, location + vector);
        }
    }

    public void Transport(Location location, Vector vector)
    {
        var actor = actors.ActorAt(location + vector);

        if (actor.Vector == vector)
        {
            if (vector.IsZero())
            {
                // Ordinarily this hangs indefinitely. We catch this situation
                // and turn it into a no-op.
                tracer.TraceCrash("Transport called with zero vector");
                return;
            }
            
            var search = actor.Location;
            var target = new Location();
            var ended = false;
            var success = true;

            while (!ended)
            {
                search += vector;
                var element = tiles.ElementAt(search);
                if (element.Id == elements.BoardEdgeId)
                {
                    ended = true;
                }
                else
                {
                    if (success)
                    {
                        success = false;
                        if (!element.IsFloor)
                        {
                            Push(search, vector);
                            element = tiles.ElementAt(search);
                        }

                        if (element.IsFloor)
                        {
                            ended = true;
                            target = search;
                        }
                        else
                        {
                            target.X = 0;
                        }
                    }
                }

                if (element.Id == elements.TransporterId)
                    if (actors.ActorAt(search).Vector == -vector)
                        success = true;
            }

            if (target.X > 0)
            {
                MoveTile(actor.Location - vector, target);
                soundUnit.PlaySound(3, sounds.Transporter);
            }
        }
    }
    
    private void MoveTile(Location source, Location target)
    {
        var sourceIndex = actors.ActorIndexAt(source);
        if (sourceIndex >= 0)
        {
            mover.MoveActor(sourceIndex, target);
        }
        else
        {
            tiles[target] = tiles[source];
            boardUpdater.UpdateBoard(target);
            tileRemover.RemoveItem(source);
            boardUpdater.UpdateBoard(source);
        }
    }
}