using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class PusherTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Pusher_ShouldMoveForward_WhenPathIsClear()
    {
        // Place a pusher.
        var pusherIndex = SpawnTo(5, 5, Elements.PusherId);
        var pusher = Actors[pusherIndex];
        pusher.Vector = Vector.East;
        pusher.Cycle = 1;

        // Wait for the pusher to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "pusher should leave previous location");
        TileAt(6, 5).Id.Should().Be(Elements.PusherId,
            "pusher should move to target location");
    }

    [Test]
    public void Pusher_ShouldPushPushableElement()
    {
        // Place a pusher.
        var pusherIndex = SpawnTo(5, 5, Elements.PusherId);
        var pusher = Actors[pusherIndex];
        pusher.Vector = Vector.East;
        pusher.Cycle = 1;

        // Place a pushable tile in front of it.
        PlotTo(6, 5, Elements.BoulderId);

        // Wait for the pusher to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "pusher should leave original position");
        TileAt(6, 5).Id.Should().Be(Elements.PusherId,
            "pusher should advance to the obstacle position");
        TileAt(7, 5).Id.Should().Be(Elements.BoulderId,
            "pushable obstacle should be displaced forward");
    }

    [Test]
    public void Pusher_ShouldBeBlocked_WhenFacingSolidObstacle()
    {
        // Place a pusher.
        var pusherIndex = SpawnTo(5, 5, Elements.PusherId);
        var pusher = Actors[pusherIndex];
        pusher.Vector = Vector.East;
        pusher.Cycle = 1;

        // Place a solid tile in front of it.
        PlotTo(6, 5, Elements.SolidId);

        // Wait for the pusher to attempt to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.PusherId,
            "pusher should remain in original position when blocked");
        TileAt(6, 5).Id.Should().Be(Elements.SolidId,
            "wall should remain");
    }

    [Test]
    public void Pusher_ShouldPushChainedPushers()
    {
        // Pushers that act while another pusher is behind them is a specially handled case.
        // If there is a pusher behind another pusher, the one behind will also move.

        // Place a pusher.
        var trailingIndex = SpawnTo(4, 5, Elements.PusherId);
        var trailingPusher = Actors[trailingIndex];
        trailingPusher.Vector = Vector.East;
        trailingPusher.Cycle = 1;

        // Place a pusher in front of it.
        var leadingIndex = SpawnTo(5, 5, Elements.PusherId);
        var leadingPusher = Actors[leadingIndex];
        leadingPusher.Vector = Vector.East;
        leadingPusher.Cycle = 1;

        // Wait for the pushers to move.
        Step();

        // Assert.
        TileAt(4, 5).Id.Should().Be(Elements.EmptyId,
            "trailing pusher should have left its old position");
        TileAt(5, 5).Id.Should().Be(Elements.PusherId,
            "trailing pusher should have moved into the leading pusher's location");
        TileAt(6, 5).Id.Should().Be(Elements.PusherId,
            "leading pusher should have moved");
    }
}