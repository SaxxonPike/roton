using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Pusher(
    ITiles tiles,
    IElementList elementList,
    IEngineAccessor engine,
    IActorList actorList,
    ISoundUnit soundUnit,
    ISounds sounds,
    IBoardUpdater boardUpdater,
    IFeatures features)
    : IPusher
{
    private IEngine Engine => engine.Instance;

    public void Push(Location location, Vector vector)
    {
        ref var tile = ref tiles[location];
        if (tile.Id == elementList.SliderEwId && vector.Y == 0 ||
            tile.Id == elementList.SliderNsId && vector.X == 0 ||
            elementList[tile.Id].IsPushable)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
                throw Exceptions.PushStackOverflow;

            ref var furtherTile = ref tiles[location + vector];
            if (furtherTile.Id == elementList.TransporterId)
                Transport(location, vector);
            else if (furtherTile.Id != elementList.EmptyId)
                Push(location + vector, vector);

            var furtherElement = elementList[furtherTile.Id];
            if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != elementList.PlayerId)
                Engine.Destroy(location + vector);

            furtherElement = elementList[furtherTile.Id];
            if (furtherElement.IsFloor)
                MoveTile(location, location + vector);
        }
    }

    public void Transport(Location location, Vector vector)
    {
        var actor = actorList.ActorAt(location + vector);

        if (actor.Vector == vector)
        {
            var search = actor.Location;
            var target = new Location();
            var ended = false;
            var success = true;

            while (!ended)
            {
                search += vector;
                var element = tiles.ElementAt(search);
                if (element.Id == elementList.BoardEdgeId)
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

                if (element.Id == elementList.TransporterId)
                    if (actorList.ActorAt(search).Vector == -vector)
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
        var sourceIndex = actorList.ActorIndexAt(source);
        if (sourceIndex >= 0)
        {
            Engine.MoveActor(sourceIndex, target);
        }
        else
        {
            tiles[target] = tiles[source];
            boardUpdater.UpdateBoard(target);
            features.RemoveItem(source);
            boardUpdater.UpdateBoard(source);
        }
    }
}