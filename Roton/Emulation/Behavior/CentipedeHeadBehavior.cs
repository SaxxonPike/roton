using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class CentipedeHeadBehavior : ElementBehavior
    {
        public override string KnownName => "Centipede (Head)";

        public override void Act(IEngine engine, int index)
        {
            var player = engine.Player;
            var actor = engine.Actors[index];

            // The centipede can randomly change direction towards the player if aligned

            if (player.Location.X == actor.Location.X && actor.P1 > engine.SyncRandomNumber(10))
            {
                actor.Vector.CopyFrom(engine.Seek(actor.Location));
            }
            else if (player.Location.Y == actor.Location.Y && actor.P1 > engine.SyncRandomNumber(10))
            {
                actor.Vector.CopyFrom(engine.Seek(actor.Location));
            }
            else if (actor.Vector.IsZero() || actor.P2 > engine.SyncRandomNumber(10) << 2)
            {
                actor.Vector.CopyFrom(engine.Rnd());
            }

            if (actor.Vector.IsNonZero())
            {
                // The centipede wants to move, determine where it can

                var vector = actor.Vector.Clone();
                var element = engine.ElementAt(actor.Location.Sum(vector));
                if (!element.IsFloor && element.Id != engine.Elements.PlayerId)
                {
                    actor.Vector.CopyFrom(engine.RndP(vector));
                    element = engine.ElementAt(actor.Location.Sum(actor.Vector));
                    if (!element.IsFloor && element.Id != engine.Elements.PlayerId)
                    {
                        actor.Vector.SetOpposite();
                        element = engine.ElementAt(actor.Location.Sum(actor.Vector));
                        if (!element.IsFloor && element.Id != engine.Elements.PlayerId)
                        {
                            actor.Vector.CopyFrom(vector.Opposite());
                            element = engine.ElementAt(actor.Location.Sum(actor.Vector));
                            if (!element.IsFloor && element.Id != engine.Elements.PlayerId)
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

                engine.TileAt(actor.Location).Id = engine.Elements.SegmentId;
                engine.UpdateBoard(actor.Location);
                var segmentIndex = index;
                while (true)
                {
                    var segment = engine.Actors[segmentIndex];
                    var i = segment.Follower;
                    segment.Follower = segment.Leader;
                    segment.Leader = i;
                    if (i > 0)
                        segmentIndex = i;
                    else
                        break;
                }
                var newHead = engine.Actors[segmentIndex];
                engine.TileAt(newHead.Location).Id = engine.Elements.HeadId;
                engine.UpdateBoard(newHead.Location);
            }
            else
            {
                // The centipede has a direction to go, so move it

                var target = actor.Location.Sum(actor.Vector);

                if (engine.ElementAt(target).Id == engine.Elements.PlayerId)
                {
                    // The centipede is moving into a player

                    if (actor.Follower > 0)
                    {
                        var follower = engine.Actors[actor.Follower];
                        engine.TileAt(follower.Location).Id = engine.Elements.HeadId;
                        follower.Leader = -1;
                        engine.UpdateBoard(follower.Location);
                    }
                    actor.Follower = -1;
                    actor.Leader = -1;
                    engine.Attack(index, target);
                }
                else
                {
                    engine.MoveActor(index, target);
                    var segmentIndex = index;

                    // The centipede has moved, so move its followers

                    do
                    {
                        var segment = engine.Actors[segmentIndex];
                        var origin = segment.Location.Difference(segment.Vector);
                        var vector = segment.Vector;

                        if (segment.Follower < 0)
                        {
                            // Determine if there are any eligible new follower segments
                            if (engine.ElementAt(origin.Difference(vector)).Id == engine.Elements.SegmentId &&
                                engine.ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = engine.ActorIndexAt(origin.Difference(vector));
                            }
                            else if (engine.ElementAt(origin.Difference(vector.Swap())).Id == engine.Elements.SegmentId &&
                                     engine.ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = engine.ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (engine.ElementAt(origin.Sum(vector.Swap())).Id == engine.Elements.SegmentId &&
                                     engine.ActorAt(origin.Sum(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = engine.ActorIndexAt(origin.Sum(vector.Swap()));
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
                            var follower = engine.Actors[followerIndex];
                            follower.Leader = segmentIndex;
                            follower.P1 = segment.P1;
                            follower.P2 = segment.P2;
                            follower.Vector.SetTo(origin.X - follower.Location.X, origin.Y - follower.Location.Y);
                            engine.MoveActor(segment.Follower, origin);
                        }

                        segmentIndex = segment.Follower;
                    } while (segmentIndex > 0);
                }
            }
        }
    }
}
