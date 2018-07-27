using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x2C)]
    [ContextEngine(ContextEngine.Super, 0x2C)]
    public sealed class CentipedeHeadAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public CentipedeHeadAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var player = Engine.Player;
            var actor = Engine.Actors[index];

            // The centipede can randomly change direction towards the player if aligned

            if (player.Location.X == actor.Location.X && actor.P1 > Engine.Random.GetNext(10))
            {
                actor.Vector.CopyFrom(Engine.Seek(actor.Location));
            }
            else if (player.Location.Y == actor.Location.Y && actor.P1 > Engine.Random.GetNext(10))
            {
                actor.Vector.CopyFrom(Engine.Seek(actor.Location));
            }
            else if (actor.Vector.IsZero() || actor.P2 > Engine.Random.GetNext(10) << 2)
            {
                actor.Vector.CopyFrom(Engine.Rnd());
            }

            if (actor.Vector.IsNonZero())
            {
                // The centipede wants to move, determine where it can

                var vector = actor.Vector.Clone();
                var element = Engine.Tiles.ElementAt(actor.Location.Sum(vector));
                if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
                {
                    actor.Vector.CopyFrom(Engine.RndP(vector));
                    element = Engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector));
                    if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
                    {
                        actor.Vector.SetOpposite();
                        element = Engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector));
                        if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
                        {
                            actor.Vector.CopyFrom(vector.Opposite());
                            element = Engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector));
                            if (!element.IsFloor && element.Id != Engine.ElementList.PlayerId)
                            {
                                actor.Vector.SetTo(0, 0);
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

                var target = actor.Location.Sum(actor.Vector);

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
                        var origin = segment.Location.Difference(segment.Vector);
                        var vector = segment.Vector;

                        if (segment.Follower < 0)
                        {
                            // Determine if there are any eligible new follower segments
                            if (Engine.Tiles.ElementAt(origin.Difference(vector)).Id == Engine.ElementList.SegmentId &&
                                Engine.ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = Engine.ActorIndexAt(origin.Difference(vector));
                            }
                            else if (Engine.Tiles.ElementAt(origin.Difference(vector.Swap())).Id == Engine.ElementList.SegmentId &&
                                     Engine.ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = Engine.ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (Engine.Tiles.ElementAt(origin.Sum(vector.Swap())).Id == Engine.ElementList.SegmentId &&
                                     Engine.ActorAt(origin.Sum(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = Engine.ActorIndexAt(origin.Sum(vector.Swap()));
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
                            follower.Vector.SetTo(origin.X - follower.Location.X, origin.Y - follower.Location.Y);
                            Engine.MoveActor(segment.Follower, origin);
                        }

                        segmentIndex = segment.Follower;
                    } while (segmentIndex > 0);
                }
            }
        }
    }
}