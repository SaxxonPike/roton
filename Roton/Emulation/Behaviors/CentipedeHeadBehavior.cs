using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class CentipedeHeadBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        private readonly IDrawer _drawer;
        private readonly ICompass _compass;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Head;

        public CentipedeHeadBehavior(IActors actors, IRandom random, IElements elements, ITiles tiles,
            IDrawer drawer, ICompass compass, IMover mover) : base(mover)
        {
            _actors = actors;
            _random = random;
            _elements = elements;
            _tiles = tiles;
            _drawer = drawer;
            _compass = compass;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var player = _actors.Player;
            var actor = _actors[index];

            // The centipede can randomly change direction towards the player if aligned

            if (player.Location.X == actor.Location.X && actor.P1 > _random.Synced(10))
            {
                actor.Vector.CopyFrom(_compass.Seek(actor.Location));
            }
            else if (player.Location.Y == actor.Location.Y && actor.P1 > _random.Synced(10))
            {
                actor.Vector.CopyFrom(_compass.Seek(actor.Location));
            }
            else if (actor.Vector.IsZero() || actor.P2 > _random.Synced(10) << 2)
            {
                actor.Vector.CopyFrom(_compass.Rnd());
            }

            if (actor.Vector.IsNonZero())
            {
                // The centipede wants to move, determine where it can

                var vector = actor.Vector.Clone();
                var element = _tiles.ElementAt(actor.Location.Sum(vector));
                if (!element.IsFloor && element.Id != _elements.PlayerId)
                {
                    actor.Vector.CopyFrom(_compass.RndP(vector));
                    element = _tiles.ElementAt(actor.Location.Sum(actor.Vector));
                    if (!element.IsFloor && element.Id != _elements.PlayerId)
                    {
                        actor.Vector.SetOpposite();
                        element = _tiles.ElementAt(actor.Location.Sum(actor.Vector));
                        if (!element.IsFloor && element.Id != _elements.PlayerId)
                        {
                            actor.Vector.CopyFrom(vector.Opposite());
                            element = _tiles.ElementAt(actor.Location.Sum(actor.Vector));
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

                _tiles[actor.Location].Id = _elements.SegmentId;
                _drawer.UpdateBoard(actor.Location);
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
                _tiles[newHead.Location].Id = _elements.HeadId;
                _drawer.UpdateBoard(newHead.Location);
            }
            else
            {
                // The centipede has a direction to go, so move it

                var target = actor.Location.Sum(actor.Vector);

                if (_tiles.ElementAt(target).Id == _elements.PlayerId)
                {
                    // The centipede is moving into a player

                    if (actor.Follower > 0)
                    {
                        var follower = _actors[actor.Follower];
                        _tiles[follower.Location].Id = _elements.HeadId;
                        follower.Leader = -1;
                        _drawer.UpdateBoard(follower.Location);
                    }

                    actor.Follower = -1;
                    actor.Leader = -1;
                    _mover.Attack(index, target);
                }
                else
                {
                    _mover.MoveActor(index, target);
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
                            if (_tiles.ElementAt(origin.Difference(vector)).Id == _elements.SegmentId &&
                                _actors.ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = _actors.ActorIndexAt(origin.Difference(vector));
                            }
                            else if (_tiles.ElementAt(origin.Difference(vector.Swap())).Id == _elements.SegmentId &&
                                     _actors.ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = _actors.ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (_tiles.ElementAt(origin.Sum(vector.Swap())).Id == _elements.SegmentId &&
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
                            _mover.MoveActor(segment.Follower, origin);
                        }

                        segmentIndex = segment.Follower;
                    } while (segmentIndex > 0);
                }
            }
        }
    }
}