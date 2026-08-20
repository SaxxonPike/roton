using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class FakeWallTests(Context context) : AllContextIntegrationTestFixture(context)
{
    [Test]
    public void FakeWall_ShouldBeWalkable()
    {
        // Move the player.
        MovePlayerTo(10, 10);
        
        // Put a fake wall to the right of the player.
        PlotTo(11, 10, ElementList.FakeId);
        
        // Move the player into the fake wall.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(11, 10).Id.Should().Be(ElementList.PlayerId,
            "player should be able to walk on fake wall");
    }

    [Test]
    public void FakeWall_ShouldShowAlert()
    {
        // Alerts are true until triggered.
        
        // Move the player.
        MovePlayerTo(10, 10);
        
        // Place fake walls to the right.
        PlotTo(11, 10, ElementList.FakeId);
        PlotTo(12, 10, ElementList.FakeId);
        
        // Move the player through the fake wall tiles.
        Type(AnsiKey.Right);
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Alerts.FakeWall.Should().BeFalse();
    }
}