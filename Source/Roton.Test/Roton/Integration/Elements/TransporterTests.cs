using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class TransporterTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Transporter_BlocksMovementFromPerpendicularSide()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the transporter right of the player, facing south.
        var index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(index, Vector.South);

        // Attempt to move the player into the transporter.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.PlayerId,
            "player should not have moved into the transporter");
    }

    [Test]
    public void Transporter_BlocksMovementFromBackSide()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the transporter right of the player, aimed at the player.
        var index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(index, Vector.West);

        // Attempt to move the player into the transporter.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.PlayerId,
            "player should not have moved into the transporter");
    }

    [Test]
    public void Transporter_ShouldLeadToBackSide_WhenBackSideIsWalkable()
    {
        // Place the player.
        MovePlayerTo(12, 10);

        // Place the transporter left of the player, aimed away from the player.
        var index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(index, Vector.West);

        // Move the player into the transporter.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.EmptyId,
            "player should have moved through the transporter");
        TileAt(10, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have moved to the back side of the transporter");
    }

    [Test]
    public void Transporter_ShouldLeadToBackSide_WhenBackSideCanBePushed()
    {
        // Place the player.
        MovePlayerTo(12, 10);

        // Place the transporter left of the player, aimed away from the player.
        var index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(index, Vector.West);

        // Place a boulder behind the transporter.
        PlotTo(10, 10, ElementList.BoulderId);

        // Move the player into the transporter.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.EmptyId,
            "player should have moved through the transporter");
        TileAt(9, 10).Id.Should().Be(ElementList.BoulderId,
            "boulder should have been pushed away from the transporter");
        TileAt(10, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have moved to the back side of the transporter");
    }

    [Test]
    public void Transporter_ShouldLeadToOtherTransporter_WhenBackSideIsBlocked_AndOtherEndIsWalkable()
    {
        // Place the player.
        MovePlayerTo(12, 10);

        // Place the right transporter.
        var t1Index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(t1Index, Vector.West);

        // Block the left side of the right transporter.
        PlotTo(10, 10, ElementList.SolidId);

        // Place the left transporter.
        var t2Index = SpawnTo(5, 10, ElementList.TransporterId);
        FaceActor(t2Index, Vector.East);

        // Move the player into the right transporter.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(4, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have moved through the transporter out the left side");
    }

    [Test]
    public void Transporter_ShouldLeadToOtherTransporter_WhenBackSideIsBlocked_AndOtherEndCanBePushed()
    {
        // Place the player.
        MovePlayerTo(12, 10);

        // Place the right transporter.
        var t1Index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(t1Index, Vector.West);

        // Block the left side of the right transporter.
        PlotTo(10, 10, ElementList.SolidId);

        // Place the left transporter.
        var t2Index = SpawnTo(5, 10, ElementList.TransporterId);
        FaceActor(t2Index, Vector.East);

        // Place a boulder on the left side of the left transporter.
        PlotTo(4, 10, ElementList.BoulderId);

        // Move the player into the right transporter.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(3, 10).Id.Should().Be(ElementList.BoulderId,
            "boulder should have been pushed left");
        TileAt(4, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have moved through teh transporter on the left side");
    }

    [Test]
    public void Transporter_ShouldBlock_WhenBackSideIsBlocked_AndNoOpposingTransporter()
    {
        // Place the player.
        MovePlayerTo(12, 10);

        // Place the transporter.
        var t1Index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(t1Index, Vector.West);

        // Block the left side of the transporter.
        PlotTo(10, 10, ElementList.SolidId);

        // Attempt to move the player into the transporter.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.PlayerId,
            "player should not have moved through the transporter");
    }

    [Test]
    public void Transporter_ShouldBlock_WhenBackSideIsBlocked_AndNoEligibleTransporter()
    {
        // Place the player.
        MovePlayerTo(12, 10);

        // Place the right transporter.
        var t1Index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(t1Index, Vector.West);

        // Block the left side of the right transporter.
        PlotTo(10, 10, ElementList.SolidId);

        // Place the left transporter.
        var t2Index = SpawnTo(5, 10, ElementList.TransporterId);
        FaceActor(t2Index, Vector.East);

        // Block the left side of the left transporter.
        PlotTo(4, 10, ElementList.SolidId);

        // Attempt to move the player into the transporter.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.PlayerId,
            "player should not have moved through the transporter");
    }

    [Test]
    public void Transporter_ShouldAllowPushablesThrough_ToBackSide()
    {
        // Place the player.
        MovePlayerTo(13, 10);

        // Place the transporter.
        var t1Index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(t1Index, Vector.West);

        // Place a boulder in front of the transporter.
        PlotTo(12, 10, ElementList.BoulderId);

        // Move the player into the boulder.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have pushed the boulder");
        TileAt(10, 10).Id.Should().Be(ElementList.BoulderId,
            "boulder should have been sent through the transporter");
    }

    [Test]
    public void Transporter_ShouldAllowPushablesThrough_ToOtherSide()
    {
        // Place the player.
        MovePlayerTo(13, 10);

        // Place the right transporter.
        var t1Index = SpawnTo(11, 10, ElementList.TransporterId);
        FaceActor(t1Index, Vector.West);

        // Block the left side of the right transporter.
        PlotTo(10, 10, ElementList.SolidId);
        
        // Place a boulder in front of the right transporter.
        PlotTo(12, 10, ElementList.BoulderId);

        // Place the left transporter.
        var t2Index = SpawnTo(5, 10, ElementList.TransporterId);
        FaceActor(t2Index, Vector.East);

        // Move the player into the boulder.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have pushed the boulder");
        TileAt(4, 10).Id.Should().Be(ElementList.BoulderId,
            "boulder should have been sent through the transporter");
    }
}