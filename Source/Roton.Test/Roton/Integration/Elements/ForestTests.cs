using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class ForestTests(Context context) : AllContextIntegrationTestFixture(context)
{
    [Test]
    public void Forest_ShouldBecomeWalkable_WhenPlayerWalksOnIt()
    {
        // The Original engine will change the tile into an empty, whereas
        // the Super engine will change it into a floor. We use the same code
        // to establish the expected behavior without checking the ID of the
        // element directly.

        // Move player.
        MovePlayerTo(10, 10);

        // Place a forest tile to the right of the player.
        PlotTo(11, 10, ElementList.ForestId);

        // Move the player through the forest tile.
        Type(AnsiKey.Right);
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(12, 10).Id.Should().Be(ElementList.PlayerId,
            "player should be able to walk on forest");
        ((bool)ElementList[TileAt(11, 10).Id].IsFloor).Should().BeTrue(
            "forest should become walkable");
    }

    [Test]
    public void Forest_ShouldShowAlert()
    {
        // Alerts are true until triggered.
        
        // Move player.
        MovePlayerTo(10, 10);

        // Place forest tiles to the right of the player.
        PlotTo(11, 10, ElementList.ForestId);
        PlotTo(12, 10, ElementList.ForestId);

        // Move the player through the forest tiles.
        Type(AnsiKey.Right);
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Alerts.Forest.Should().BeFalse();
    }
}