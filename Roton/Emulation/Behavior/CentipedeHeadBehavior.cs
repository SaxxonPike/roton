using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class CentipedeHeadBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly IEngine _engine;
        private readonly IElements _elements;
        private readonly IGrid _grid;

        public override string KnownName => KnownNames.Head;

        public CentipedeHeadBehavior(IActors actors, IRandom random, IEngine engine, IElements elements, IGrid grid) : base(engine)
        {
            _actors = actors;
            _random = random;
            _engine = engine;
            _elements = elements;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            var player = _actors.Player;
            var actor = _actors[index];

            // The centipede can randomly change direction towards the player if aligned

            if (player.Location.X == actor.Location.X && actor.P1 > _random.Synced(10))
            {
                actor.Vector.CopyFrom(_engine.Seek(actor.Location));
            }
            else if (player.Location.Y == actor.Location.Y && actor.P1 > _random.Synced(10))
            {
                actor.Vector.CopyFrom(_engine.Seek(actor.Location));
            }
            else if (actor.Vector.IsZero() || actor.P2 > _random.Synced(10) << 2)
            {
                actor.Vector.CopyFrom(_engine.Rnd());
            }

            if (actor.Vector.IsNonZero())
            {
                // The centipede wants to move, determine where it can

                var vector = actor.Vector.Clone();
                var element = _grid.ElementAt(actor.Location.Sum(vector));
                if (!element.IsFloor && element.Id != _elements.PlayerId)
                {
                    actor.Vector.CopyFrom(_engine.RndP(vector));
                    element = _grid.ElementAt(actor.Location.Sum(actor.Vector));
                    if (!element.IsFloor && element.Id != _elements.PlayerId)
                    {
                        actor.Vector.SetOpposite();
                        element = _grid.ElementAt(actor.Location.Sum(actor.Vector));
                        if (!element.IsFloor && element.Id != _elements.PlayerId)
                        {
                            actor.Vector.CopyFrom(vector.Opposite());
                            element = _grid.ElementAt(actor.Location.Sum(actor.Vector));
                            if (!element.IsFloor && element.Id != _elements.PlayerId)
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

                _grid.TileAt(actor.Location).Id = _elements.SegmentId;
                _engine.UpdateBoard(actor.Location);
                var segmentIndex = index;
                while (true)
                {
                    var segment = _actors[segmentIndex];
                    var i = segment.Follower;
                    segment.Follower = segment.Leader;
                    segment.Leader = i;
                    if (i > 0)
                        segmentIndex = i;
                    else
                        break;
                }
                var newHead = _actors[segmentIndex];
                _grid.TileAt(newHead.Location).Id = _elements.HeadId;
                _engine.UpdateBoard(newHead.Location);
            }
            else
            {
                // The centipede has a direction to go, so move it

                var target = actor.Location.Sum(actor.Vector);

                if (_grid.ElementAt(target).Id == _elements.PlayerId)
                {
                    // The centipede is moving into a player

                    if (actor.Follower > 0)
                    {
                        var follower = _actors[actor.Follower];
                        _grid.TileAt(follower.Location).Id = _elements.HeadId;
                        follower.Leader = -1;
                        _engine.UpdateBoard(follower.Location);
                    }
                    actor.Follower = -1;
                    actor.Leader = -1;
                    _engine.Attack(index, target);
                }
                else
                {
                    _engine.MoveActor(index, target);
                    var segmentIndex = index;

                    // The centipede has moved, so move its followers

                    do
                    {
                        var segment = _actors[segmentIndex];
                        var origin = segment.Location.Difference(segment.Vector);
                        var vector = segment.Vector;

                        if (segment.Follower < 0)
                        {
                            // Determine if there are any eligible new follower segments
                            if (_grid.ElementAt(origin.Difference(vector)).Id == _elements.SegmentId &&
                                _actors.ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = _actors.ActorIndexAt(origin.Difference(vector));
                            }
                            else if (_grid.ElementAt(origin.Difference(vector.Swap())).Id == _elements.SegmentId &&
                                     _actors.ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = _actors.ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (_grid.ElementAt(origin.Sum(vector.Swap())).Id == _elements.SegmentId &&
                                     _actors.ActorAt(origin.Sum(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = _actors.ActorIndexAt(origin.Sum(vector.Swap()));
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
                            var follower = _actors[followerIndex];
                            follower.Leader = segmentIndex;
                            follower.P1 = segment.P1;
                            follower.P2 = segment.P2;
                            follower.Vector.SetTo(origin.X - follower.Location.X, origin.Y - follower.Location.Y);
                            _engine.MoveActor(segment.Follower, origin);
                        }

                        segmentIndex = segment.Follower;
                    } while (segmentIndex > 0);
                }
            }
        }
    }
}