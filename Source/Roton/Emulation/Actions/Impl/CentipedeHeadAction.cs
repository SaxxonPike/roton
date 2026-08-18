using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x2C)]
[Context(Context.Super, 0x2C)]
public sealed class CentipedeHeadAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var player = Engine.Player;
        var actor = Engine.Actors[index];

        // The centipede can randomly change direction towards the player if aligned

        if (player.Location.X == actor.Location.X && actor.P1 > Engine.Random.GetNext(10))
        {
            actor.Vector = Engine.Seek(actor.Location);
        }
        else if (player.Location.Y == actor.Location.Y && actor.P1 > Engine.Random.GetNext(10))
        {
            actor.Vector = Engine.Seek(actor.Location);
        }
        else if (actor.Vector.IsZero() || actor.P2 > Engine.Random.GetNext(10) << 2)
        {
            actor.Vector = Engine.Rnd();
        }

        if (actor.Vector.IsNonZero())
        {
            // The centipede wants to move, determine where it can

            var vector = actor.Vector;
            var element = Engine.Tiles.ElementAt(actor.Location + actor.Vector);
            if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
            {
                actor.Vector = Engine.RndP(vector);
                element = Engine.Tiles.ElementAt(actor.Location + actor.Vector);
                if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
                {
                    actor.Vector = -actor.Vector;
                    element = Engine.Tiles.ElementAt(actor.Location + actor.Vector);
                    if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
                    {
                        actor.Vector = -vector;
                        element = Engine.Tiles.ElementAt(actor.Location + actor.Vector);
                        if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
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

            Engine.Tiles[actor.Location].Id = Engine.ElementList.SegmentId;
            Engine.UpdateBoard(actor.Location);
            var segmentIndex = index;
            while (true)
            {
                var segment = Engine.Actors[segmentIndex];
                var i = segment.Follower;
                segment.Follower = segment.Leader;
                segment.Leader = i;
                if (i > 0)
                    segmentIndex = i;
                else
                    break;
            }

            var newHead = Engine.Actors[segmentIndex];
            Engine.Tiles[newHead.Location].Id = Engine.ElementList.HeadId;
            Engine.UpdateBoard(newHead.Location);
        }
        else
        {
            // The centipede has a direction to go, so move it

            var target = actor.Location + actor.Vector;

            if (Engine.Tiles.ElementAt(target).Id == Engine.ElementList.PlayerId)
            {
                // The centipede is moving into a player

                if (actor.Follower > 0)
                {
                    var follower = Engine.Actors[actor.Follower];
                    Engine.Tiles[follower.Location].Id = Engine.ElementList.HeadId;
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
                    var segment = Engine.Actors[segmentIndex];
                    var origin = segment.Location - segment.Vector;
                    var vector = segment.Vector;

                    if (segment.Follower < 0)
                    {
                        // Determine if there are any eligible new follower segments
                        if (Engine.Tiles.ElementAt(origin - vector).Id == Engine.ElementList.SegmentId &&
                            Engine.ActorAt(origin - vector).Leader <= 0)
                        {
                            segment.Follower = Engine.ActorIndexAt(origin - vector);
                        }
                        else if (Engine.Tiles.ElementAt(origin - vector.Swap()).Id == Engine.ElementList.SegmentId &&
                                 Engine.ActorAt(origin - vector.Swap()).Leader <= 0)
                        {
                            segment.Follower = Engine.ActorIndexAt(origin - vector.Swap());
                        }
                        else if (Engine.Tiles.ElementAt(origin + vector.Swap()).Id == Engine.ElementList.SegmentId &&
                                 Engine.ActorAt(origin + vector.Swap()).Leader <= 0)
                        {
                            segment.Follower = Engine.ActorIndexAt(origin + vector.Swap());
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
                        var follower = Engine.Actors[followerIndex];
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