using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x2C)]
[Context(Context.Super, 0x2C)]
public sealed class CentipedeHeadAction(
    IEngineAccessor engine,
    IActorList actorList,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elementList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var player = actorList.Player;
        var actor = actorList[index];

        // The centipede can randomly change direction towards the player if aligned

        if (player.Location.X == actor.Location.X && actor.P1 > randomizer.GetNext(10))
        {
            actor.Vector = Engine.Seek(actor.Location);
        }
        else if (player.Location.Y == actor.Location.Y && actor.P1 > randomizer.GetNext(10))
        {
            actor.Vector = Engine.Seek(actor.Location);
        }
        else if (actor.Vector.IsZero() || actor.P2 > randomizer.GetNext(10) << 2)
        {
            actor.Vector = Engine.Rnd();
        }

        if (actor.Vector.IsNonZero())
        {
            // The centipede wants to move, determine where it can

            var vector = actor.Vector;
            var element = tiles.ElementAt(actor.Location + actor.Vector);
            if (!element.IsFloor && element.Id != elementList.PlayerId)
            {
                actor.Vector = Engine.RndP(vector);
                element = tiles.ElementAt(actor.Location + actor.Vector);
                if (!element.IsFloor && element.Id != elementList.PlayerId)
                {
                    actor.Vector = -actor.Vector;
                    element = tiles.ElementAt(actor.Location + actor.Vector);
                    if (!element.IsFloor && element.Id != elementList.PlayerId)
                    {
                        actor.Vector = -vector;
                        element = tiles.ElementAt(actor.Location + actor.Vector);
                        if (!element.IsFloor && element.Id != elementList.PlayerId)
                        {
                            actor.Vector = Vector.Idle;
                        }
                    }
                }
            }
        }

        if (actor.Vector.IsZero())
        {
            // Reverse the centipede

            tiles[actor.Location].Id = elementList.SegmentId;
            Engine.UpdateBoard(actor.Location);
            var segmentIndex = index;
            while (true)
            {
                var segment = actorList[segmentIndex];
                var i = segment.Follower;
                segment.Follower = segment.Leader;
                segment.Leader = i;
                if (i > 0)
                    segmentIndex = i;
                else
                    break;
            }

            var newHead = actorList[segmentIndex];
            tiles[newHead.Location].Id = elementList.HeadId;
            Engine.UpdateBoard(newHead.Location);
        }
        else
        {
            // The centipede has a direction to go, so move it

            var target = actor.Location + actor.Vector;

            if (tiles.ElementAt(target).Id == elementList.PlayerId)
            {
                // The centipede is moving into a player

                if (actor.Follower > 0)
                {
                    var follower = actorList[actor.Follower];
                    tiles[follower.Location].Id = elementList.HeadId;
                    follower.Leader = -1;
                    Engine.UpdateBoard(follower.Location);
                }

                actor.Follower = -1;
                actor.Leader = -1;
                Engine.Attack(index, target);
            }
            else
            {
                Engine.MoveActor(index, target);
                var segmentIndex = index;

                // The centipede has moved, so move its followers

                do
                {
                    var segment = actorList[segmentIndex];
                    var origin = segment.Location - segment.Vector;
                    var vector = segment.Vector;

                    if (segment.Follower < 0)
                    {
                        // Determine if there are any eligible new follower segments
                        if (tiles.ElementAt(origin - vector).Id == elementList.SegmentId &&
                            actorList.ActorAt(origin - vector).Leader <= 0)
                        {
                            segment.Follower = actorList.ActorIndexAt(origin - vector);
                        }
                        else if (tiles.ElementAt(origin - vector.Swap()).Id == elementList.SegmentId &&
                                 actorList.ActorAt(origin - vector.Swap()).Leader <= 0)
                        {
                            segment.Follower = actorList.ActorIndexAt(origin - vector.Swap());
                        }
                        else if (tiles.ElementAt(origin + vector.Swap()).Id == elementList.SegmentId &&
                                 actorList.ActorAt(origin + vector.Swap()).Leader <= 0)
                        {
                            segment.Follower = actorList.ActorIndexAt(origin + vector.Swap());
                        }
                        else
                        {
                            segment.Follower = -1;
                        }
                    }

                    // Move follower segment
                    var followerIndex = segment.Follower;
                    if (followerIndex == segmentIndex)
                    {
                        throw Exceptions.SelfReferenceCentipede;
                    }

                    if (followerIndex > 0)
                    {
                        var follower = actorList[followerIndex];
                        follower.Leader = segmentIndex;
                        follower.P1 = segment.P1;
                        follower.P2 = segment.P2;
                        follower.Vector = new Vector(origin.X - follower.Location.X, origin.Y - follower.Location.Y);
                        Engine.MoveActor(segment.Follower, origin);
                    }

                    segmentIndex = segment.Follower;
                } while (segmentIndex > 0);
            }
        }
    }
}