using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the centipede head element.
/// </summary>
[Context(Context.Original, 0x2C)]
[Context(Context.Super, 0x2C)]
internal sealed class CentipedeHeadAction(
    IActorList actors,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elements,
    IBoardUpdater boardUpdater,
    IMover mover,
    INavigator navigator,
    IAttacker attacker)
    : IAction
{
    public void Act(int index)
    {
        var player = actors.Player;
        var actor = actors[index];

        // The centipede can randomly change direction towards the player if aligned

        if (player.Location.X == actor.Location.X && actor.P1 > randomizer.GetNext(10))
        {
            actor.Vector = navigator.Seek(actor.Location);
        }
        else if (player.Location.Y == actor.Location.Y && actor.P1 > randomizer.GetNext(10))
        {
            actor.Vector = navigator.Seek(actor.Location);
        }
        else if (actor.Vector.IsZero() || actor.P2 > randomizer.GetNext(10) << 2)
        {
            actor.Vector = navigator.Rnd();
        }

        if (actor.Vector.IsNonZero())
        {
            // The centipede wants to move, determine where it can

            var vector = actor.Vector;
            var element = tiles.ElementAt(actor.Location + actor.Vector);
            if (!element.IsFloor && element.Id != elements.PlayerId)
            {
                actor.Vector = navigator.RndP(vector);
                element = tiles.ElementAt(actor.Location + actor.Vector);
                if (!element.IsFloor && element.Id != elements.PlayerId)
                {
                    actor.Vector = -actor.Vector;
                    element = tiles.ElementAt(actor.Location + actor.Vector);
                    if (!element.IsFloor && element.Id != elements.PlayerId)
                    {
                        actor.Vector = -vector;
                        element = tiles.ElementAt(actor.Location + actor.Vector);
                        if (!element.IsFloor && element.Id != elements.PlayerId)
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

            tiles[actor.Location].Id = elements.SegmentId;
            boardUpdater.UpdateBoard(actor.Location);
            var segmentIndex = index;
            while (true)
            {
                var segment = actors[segmentIndex];
                var i = segment.Follower;
                segment.Follower = segment.Leader;
                segment.Leader = i;
                if (i > 0)
                    segmentIndex = i;
                else
                    break;
            }

            var newHead = actors[segmentIndex];
            tiles[newHead.Location].Id = elements.HeadId;
            boardUpdater.UpdateBoard(newHead.Location);
        }
        else
        {
            // The centipede has a direction to go, so move it

            var target = actor.Location + actor.Vector;

            if (tiles.ElementAt(target).Id == elements.PlayerId)
            {
                // The centipede is moving into a player

                if (actor.Follower > 0)
                {
                    var follower = actors[actor.Follower];
                    tiles[follower.Location].Id = elements.HeadId;
                    follower.Leader = -1;
                    boardUpdater.UpdateBoard(follower.Location);
                }

                actor.Follower = -1;
                actor.Leader = -1;
                attacker.Attack(index, target);
            }
            else
            {
                mover.MoveActor(index, target);
                var segmentIndex = index;

                // The centipede has moved, so move its followers

                do
                {
                    var segment = actors[segmentIndex];
                    var origin = segment.Location - segment.Vector;
                    var vector = segment.Vector;

                    if (segment.Follower < 0)
                    {
                        // Determine if there are any eligible new follower segments
                        if (tiles.ElementAt(origin - vector).Id == elements.SegmentId &&
                            actors.ActorAt(origin - vector).Leader <= 0)
                        {
                            segment.Follower = actors.ActorIndexAt(origin - vector);
                        }
                        else if (tiles.ElementAt(origin - vector.Swap()).Id == elements.SegmentId &&
                                 actors.ActorAt(origin - vector.Swap()).Leader <= 0)
                        {
                            segment.Follower = actors.ActorIndexAt(origin - vector.Swap());
                        }
                        else if (tiles.ElementAt(origin + vector.Swap()).Id == elements.SegmentId &&
                                 actors.ActorAt(origin + vector.Swap()).Leader <= 0)
                        {
                            segment.Follower = actors.ActorIndexAt(origin + vector.Swap());
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
                        // Ordinarily this will cause a hang, but we will detect this
                        // situation and bail out instead.
                        break;
                    }

                    if (followerIndex > 0)
                    {
                        var follower = actors[followerIndex];
                        follower.Leader = segmentIndex;
                        follower.P1 = segment.P1;
                        follower.P2 = segment.P2;
                        follower.Vector = new Vector(origin.X - follower.Location.X, origin.Y - follower.Location.Y);
                        mover.MoveActor(segment.Follower, origin);
                    }

                    segmentIndex = segment.Follower;
                } while (segmentIndex > 0);
            }
        }
    }
}