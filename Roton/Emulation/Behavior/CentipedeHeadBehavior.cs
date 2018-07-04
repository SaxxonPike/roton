using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class CentipedeHeadBehavior : EnemyBehavior
    {
        private readonly IActorList _actorList;
        private readonly IRandomizerService _randomizerService;
        private readonly IEngine _engine;
        private readonly IElementList _elementList;

        public override string KnownName => KnownNames.Head;

        public CentipedeHeadBehavior(IActorList actorList, IRandomizerService randomizerService, IEngine engine, IElementList elementList)
        {
            _actorList = actorList;
            _randomizerService = randomizerService;
            _engine = engine;
            _elementList = elementList;
        }
        
        public override void Act(int index)
        {
            var player = _actorList.GetPlayer();
            var actor = _actorList[index];

            // The centipede can randomly change direction towards the player if aligned

            if (player.Location.X == actor.Location.X && actor.P1 > _randomizerService.SyncRandom(10))
            {
                actor.Vector.CopyFrom(_engine.Seek(actor.Location));
            }
            else if (player.Location.Y == actor.Location.Y && actor.P1 > _randomizerService.SyncRandom(10))
            {
                actor.Vector.CopyFrom(_engine.Seek(actor.Location));
            }
            else if (actor.Vector.IsZero() || actor.P2 > _randomizerService.SyncRandom(10) << 2)
            {
                actor.Vector.CopyFrom(_engine.Rnd());
            }

            if (actor.Vector.IsNonZero())
            {
                // The centipede wants to move, determine where it can

                var vector = actor.Vector.Clone();
                var element = _engine.ElementAt(actor.Location.Sum(vector));
                if (!element.IsFloor && element.Id != _elementList.PlayerId)
                {
                    actor.Vector.CopyFrom(_engine.RndP(vector));
                    element = _engine.ElementAt(actor.Location.Sum(actor.Vector));
                    if (!element.IsFloor && element.Id != _elementList.PlayerId)
                    {
                        actor.Vector.SetOpposite();
                        element = _engine.ElementAt(actor.Location.Sum(actor.Vector));
                        if (!element.IsFloor && element.Id != _elementList.PlayerId)
                        {
                            actor.Vector.CopyFrom(vector.Opposite());
                            element = _engine.ElementAt(actor.Location.Sum(actor.Vector));
                            if (!element.IsFloor && element.Id != _elementList.PlayerId)
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

                _engine.TileAt(actor.Location).Id = _elementList.SegmentId;
                _engine.UpdateBoard(actor.Location);
                var segmentIndex = index;
                while (true)
                {
                    var segment = _actorList[segmentIndex];
                    var i = segment.Follower;
                    segment.Follower = segment.Leader;
                    segment.Leader = i;
                    if (i > 0)
                        segmentIndex = i;
                    else
                        break;
                }
                var newHead = _actorList[segmentIndex];
                _engine.TileAt(newHead.Location).Id = _elementList.HeadId;
                _engine.UpdateBoard(newHead.Location);
            }
            else
            {
                // The centipede has a direction to go, so move it

                var target = actor.Location.Sum(actor.Vector);

                if (_engine.ElementAt(target).Id == _elementList.PlayerId)
                {
                    // The centipede is moving into a player

                    if (actor.Follower > 0)
                    {
                        var follower = _actorList[actor.Follower];
                        _engine.TileAt(follower.Location).Id = _elementList.HeadId;
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
                        var segment = _actorList[segmentIndex];
                        var origin = segment.Location.Difference(segment.Vector);
                        var vector = segment.Vector;

                        if (segment.Follower < 0)
                        {
                            // Determine if there are any eligible new follower segments
                            if (_engine.ElementAt(origin.Difference(vector)).Id == _elementList.SegmentId &&
                                _engine.ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = _engine.ActorIndexAt(origin.Difference(vector));
                            }
                            else if (_engine.ElementAt(origin.Difference(vector.Swap())).Id == _elementList.SegmentId &&
                                     _engine.ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = _engine.ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (_engine.ElementAt(origin.Sum(vector.Swap())).Id == _elementList.SegmentId &&
                                     _engine.ActorAt(origin.Sum(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = _engine.ActorIndexAt(origin.Sum(vector.Swap()));
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
                            var follower = _actorList[followerIndex];
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