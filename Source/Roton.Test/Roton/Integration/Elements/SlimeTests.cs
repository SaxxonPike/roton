using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class SlimeTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Slime_ShouldSpreadToSurroundingFloors_WhenTimerReachesThreshold()
    {
        // Create the slime.
        var slimeIndex = SpawnTo(5, 5, Elements.SlimeId, 0x0A);
        var slime = Actors[slimeIndex];
        slime.P1 = 1;
        slime.P2 = 1;
        slime.Cycle = 1;

        // Wait for the slime to spread.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.BreakableId,
            "origin tile should turn into a breakable slime trail");
        TileAt(5, 5).Color.Should().Be(0x0A,
            "slime trail color should match origin slime color");

        (TileAt(5, 4).Id == Elements.SlimeId &&
         TileAt(5, 6).Id == Elements.SlimeId &&
         TileAt(4, 5).Id == Elements.SlimeId &&
         TileAt(6, 5).Id == Elements.SlimeId)
            .Should().BeTrue("slime should have expanded into all adjacent tiles");
    }

    [Test]
    public void Slime_ShouldBecomeBreakableWall_WhenSurroundedByWalls()
    {
        // Create the slime.
        var slimeIndex = SpawnTo(5, 5, Elements.SlimeId, 0x0A);
        var slime = Actors[slimeIndex];
        slime.P1 = 1;
        slime.P2 = 1;
        slime.Cycle = 1;

        // Surround the slime with walls.
        PlotTo(5, 4, Elements.SolidId);
        PlotTo(5, 6, Elements.SolidId);
        PlotTo(4, 5, Elements.SolidId);
        PlotTo(6, 5, Elements.SolidId);

        // Wait for the slime to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.BreakableId,
            "surrounded slime should die and leave behind a breakable tile");
    }
}
