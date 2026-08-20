using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class PushableTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void SliderNs_ShouldPush_WhenPushedFromNorthOrSouth()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the slider to the south of the player.
        PlotTo(10, 11, ElementList.SliderNsId);

        // Move the player into the slider.
        Type(AnsiKey.Down);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.EmptyId,
            "player should not be in the original position");
        TileAt(10, 11).Id.Should().Be(ElementList.PlayerId,
            "player should have moved in the push direction");
        TileAt(10, 12).Id.Should().Be(ElementList.SliderNsId,
            "slider should have moved in the push direction");
    }

    [Test]
    public void SliderNs_ShouldNotPush_WhenPushedFromEastOrWest()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the slider to the east of the player.
        PlotTo(11, 10, ElementList.SliderNsId);

        // Move the player into the slider.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.PlayerId,
            "player should not have moved");
        TileAt(11, 10).Id.Should().Be(ElementList.SliderNsId,
            "slider should not have moved");
    }

    [Test]
    public void SliderEw_ShouldNotPush_WhenPushedFromNorthOrSouth()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the slider to the south of the player.
        PlotTo(10, 11, ElementList.SliderEwId);

        // Move the player into the slider.
        Type(AnsiKey.Down);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.PlayerId,
            "player should not have moved");
        TileAt(10, 11).Id.Should().Be(ElementList.SliderEwId,
            "slider should not have moved");
    }

    [Test]
    public void SliderEw_ShouldPush_WhenPushedFromEastOrWest()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the slider to the east of the player.
        PlotTo(11, 10, ElementList.SliderEwId);

        // Move the player into the slider.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(ElementList.EmptyId,
            "player should not be in the original position");
        TileAt(11, 10).Id.Should().Be(ElementList.PlayerId,
            "player should have moved in the push direction");
        TileAt(12, 10).Id.Should().Be(ElementList.SliderEwId,
            "slider should have moved in the push direction");
    }

    [Test]
    public void Pusher_ShouldPushGem_WhenNotBlocked()
    {
        // Place the player.
        MovePlayerTo(10, 10);
        
        // Place the gem.
        PlotTo(5, 5, ElementList.GemId);
        
        // Place a boulder beyond the gem.
        PlotTo(6, 5, ElementList.BoulderId);
        
        // Place the pusher.
        var index = SpawnTo(4, 5, ElementList.PusherId);
        FaceActor(index, Vector.East);
        Actors[index].Cycle = 1;
        
        // Run the cycle.
        Step();
        
        // Assert.
        TileAt(4, 5).Id.Should().Be(ElementList.EmptyId,
            "pusher should have left original position");
        TileAt(5, 5).Id.Should().Be(ElementList.PusherId,
            "pusher should have moved in the push direction");
        TileAt(6, 5).Id.Should().Be(ElementList.GemId,
            "gem should have been pushed");
        TileAt(7, 5).Id.Should().Be(ElementList.BoulderId,
            "boulder should have been pushed");
    }

    [Test]
    public void Pusher_ShouldCrushGem_WhenBlockedAndNotAdjacentToPusher()
    {
        // Place the player.
        MovePlayerTo(10, 10);
        
        // Place a boulder.
        PlotTo(5, 5, ElementList.BoulderId);
        
        // Place a gem beyond the boulder.
        PlotTo(6, 5, ElementList.GemId);
        
        // Place a wall beyond the gem.
        PlotTo(7, 5, ElementList.SolidId);
        
        // Place the pusher.
        var index = SpawnTo(4, 5, ElementList.PusherId);
        FaceActor(index, Vector.East);
        Actors[index].Cycle = 1;
        
        // Run the cycle.
        Step();
        
        // Assert.
        TileAt(4, 5).Id.Should().Be(ElementList.EmptyId,
            "pusher should have left original position");
        TileAt(5, 5).Id.Should().Be(ElementList.PusherId,
            "pusher should have moved in the push direction");
        TileAt(6, 5).Id.Should().Be(ElementList.BoulderId,
            "gem should have been crushed");
    }

    [Test]
    public void Pusher_ShouldNotCrushGem_WhenBlockedAndGemIsAdjacentToPusher()
    {
        // This tests an unusual behavior when the pushed element is both destructible and pushable.
        // These items are normally crushed, but if it's at the beginning of a push chain, it is not
        // considered for destruction. A pusher doesn't begin push chains with itself, it begins the
        // chain with the tile directly in front of it.

        // Place the player.
        MovePlayerTo(10, 10);
        
        // Place a gem.
        PlotTo(5, 5, ElementList.GemId);
        
        // Place a boulder beyond the gem.
        PlotTo(6, 5, ElementList.BoulderId);
        
        // Place a wall beyond the boulder.
        PlotTo(7, 5, ElementList.SolidId);
        
        // Place the pusher.
        var index = SpawnTo(4, 5, ElementList.PusherId);
        FaceActor(index, Vector.East);
        Actors[index].Cycle = 1;
        
        // Run the cycle.
        Step();
        
        // Assert.
        TileAt(4, 5).Id.Should().Be(ElementList.PusherId,
            "pusher should not have moved");
        TileAt(5, 5).Id.Should().Be(ElementList.GemId,
            "gem should not have been crushed");
        TileAt(6, 5).Id.Should().Be(ElementList.BoulderId,
            "boulder should not have moved");
    }
}