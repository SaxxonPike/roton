using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class CentipedeTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void CentipedeHead_ShouldMoveForward_WithoutSegments()
    {
        const int x = 10;
        const int y = 10;
        const int targetX = 11;
        const int targetY = 10;

        // Create the centipede head.
        var index = SpawnTo(x, y, ElementList.HeadId);
        var actor = Actors[index];
        actor.Cycle = 1;

        // Send the centipede east.
        actor.Vector = Vector.East;

        // Turn intelligence/deviance to zero so that randomness doesn't
        // mess with the test.
        actor.P1 = 0;
        actor.P2 = 0;

        // Execute.
        Step();

        // Assert.
        TileAt(x, y).Id.Should().Be(ElementList.EmptyId,
            "centipede head must move");
        TileAt(targetX, targetY).Id.Should().Be(ElementList.HeadId,
            "centipede head must have moved in its current vector");
    }

    [Test]
    public void CentipedeHead_ShouldMoveForward_WithSegments()
    {
        const int x = 10;
        const int y = 10;

        // Create a centipede head.
        var headIndex = SpawnTo(x, y, ElementList.HeadId);
        var headActor = Actors[headIndex];
        headActor.Cycle = 1;
        headActor.P1 = 0;
        headActor.P2 = 0;

        // Create a centipede segment that follows the head.
        var segmentIndex = SpawnTo(x - 1, y, ElementList.SegmentId);
        var segmentActor = Actors[segmentIndex];
        segmentActor.Cycle = 1;

        // Turn intelligence/deviance to zero so that randomness doesn't
        // mess with the test.
        headActor.P1 = 0;
        headActor.P2 = 0;

        // Send the centipede east.
        headActor.Vector = Vector.East;

        // Execute.
        Step();

        // Assert.
        TileAt(x + 1, y).Id.Should().Be(ElementList.HeadId,
            "centipede head must move");
        TileAt(x, y).Id.Should().Be(ElementList.SegmentId,
            "centipede segment must follow the head");
    }

    [Test]
    public void CentipedeHead_ShouldReverse_WhenBlockedInAllDirections()
    {
        // Create the centipede, which consists of a head and two segments.
        var headIndex = SpawnTo(10, 10, ElementList.HeadId);
        var seg1Index = SpawnTo(9, 10, ElementList.SegmentId);
        var seg2Index = SpawnTo(8, 10, ElementList.SegmentId);

        var head = Actors[headIndex];
        var seg1 = Actors[seg1Index];
        var seg2 = Actors[seg2Index];

        head.Follower = seg1Index;
        head.Cycle = 1;
        head.P1 = 0;
        head.P2 = 0;
        seg1.Leader = headIndex;
        seg1.Follower = seg2Index;
        seg2.Leader = seg1Index;
        seg2.Follower = -1;

        // Send the centipede east.
        head.Vector = Vector.East;

        // Place walls on all sides of the head.
        PlotTo(11, 10, ElementList.SolidId);
        PlotTo(10, 9, ElementList.SolidId);
        PlotTo(10, 11, ElementList.SolidId);

        // Execute.
        Step();

        // Assert board elements.
        TileAt(10, 10).Id.Should().Be(ElementList.SegmentId,
            "centipede head should become segment");
        TileAt(9, 10).Id.Should().Be(ElementList.SegmentId,
            "center of centipede should remain segment");
        TileAt(8, 10).Id.Should().Be(ElementList.HeadId,
            "rear of centipede should become head");

        // Assert centipede linkage.
        var newHead = Actors[seg2Index];
        var mid = Actors[seg1Index];
        var newTail = Actors[headIndex];

        newHead.Follower.Should().Be(seg1Index,
            "centipede head should lead its attached segment");
        mid.Leader.Should().Be(seg2Index,
            "center segment should follow its head");
        mid.Follower.Should().Be(headIndex,
            "center segment should lead its following segment");
        newTail.Leader.Should().Be(seg1Index,
            "rear segment should follow its leader");
        newTail.Follower.Should().Be(-1,
            "rear segment should not have a follower");
    }

    [Test]
    public void CentipedeHead_ShouldTurn_WhenBlockedInForwardDirection()
    {
        // Create the centipede head.
        var index = SpawnTo(10, 10, ElementList.HeadId);
        var actor = Actors[index];
        actor.Cycle = 1;
        actor.P1 = 0;
        actor.P2 = 0;

        // Turn intelligence/deviance to zero so that randomness doesn't
        // mess with the test.
        actor.P1 = 0;
        actor.P2 = 0;

        // Send the centipede head east.
        actor.Vector = Vector.East;

        // Block the east and north so that the centipede will turn south.
        PlotTo(11, 10, ElementList.SolidId);
        PlotTo(10, 9, ElementList.SolidId);

        // Execute.
        Step();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.EmptyId,
            "centipede head must move");
        TileAt(10, 11).Id.Should().Be(ElementList.HeadId,
            "centipede head must have moved south");
    }

    [Test]
    public void CentipedeSegment_ShouldBecomeHead_WhenIsolated()
    {
        // Create a centipede with three segments. This is what
        // you get when you destroy the head.
        var headIndex = SpawnTo(10, 10, ElementList.SegmentId);
        var seg1Index = SpawnTo(9, 10, ElementList.SegmentId);
        var seg2Index = SpawnTo(8, 10, ElementList.SegmentId);

        var head = Actors[headIndex];
        var seg1 = Actors[seg1Index];
        var seg2 = Actors[seg2Index];

        head.Leader = -1;
        head.Follower = seg1Index;
        head.Cycle = 1;
        head.P1 = 0;
        head.P2 = 0;

        seg1.Leader = headIndex;
        seg1.Follower = seg2Index;
        seg1.Cycle = 1;

        seg2.Leader = seg1Index;
        seg2.Follower = -1;
        seg2.Cycle = 1;
        
        // Guarantee that the head will move east by placing walls to the north and south.
        PlotTo(10, 9, ElementList.SolidId);
        PlotTo(10, 11, ElementList.SolidId);

        // Execute the first phase.
        Step();

        // Segments should not have moved yet.
        TileAt(10, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 0 should not have moved without an established head");
        TileAt(9, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 1 should not have moved without an established head");
        TileAt(8, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 2 should not have moved without an established head");

        // Execute the second phase.
        Step();
        
        // Head should be established.
        TileAt(10, 10).Id.Should().Be(ElementList.HeadId,
            "segment 0 should have become the head");
        TileAt(9, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 1 should not have moved yet");
        TileAt(8, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 2 should not have moved yet");
        
        // Execute the third phase.
        Step();
        
        // Assert that the centipede is now moving again.
        TileAt(11, 10).Id.Should().Be(ElementList.HeadId,
            "head should have moved");
        TileAt(10, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 1 should have followed the head");
        TileAt(9, 10).Id.Should().Be(ElementList.SegmentId,
            "segment 2 should have followed segment 1");
    }
}