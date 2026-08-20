using AwesomeAssertions;
using NUnit.Framework;

namespace Roton.Test.Roton.Integration.Elements;

public class BearTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Bear_ShouldMoveTowardsPlayer_WhenWithinSensitivityRange()
    {
        // Place the player.
        MovePlayerTo(10, 6);

        // Spawn a bear nearby at max sensitivity.
        var bearIndex = SpawnTo(5, 5, ElementList.BearId);
        var bear = Actors[bearIndex];
        bear.P1 = 0;
        bear.Cycle = 1;

        // Wait for the bear to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bear should leave initial location");
        TileAt(6, 5).Id.Should().Be(ElementList.BearId,
            "bear should towards player");
    }

    [Test]
    public void Bear_ShouldNotMove_WhenNotWithinSensitivityRange()
    {
        // Place the player.
        MovePlayerTo(10, 6);

        // Spawn a bear nearby at min sensitivity.
        var bearIndex = SpawnTo(5, 5, ElementList.BearId);
        var bear = Actors[bearIndex];
        bear.P1 = 8;
        bear.Cycle = 1;

        // Wait for the bear to not move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.BearId,
            "bear should not move");
    }

    [Test]
    public void Bear_ShouldAttackPlayer_WhenAdjacent()
    {
        // Place the player.
        MovePlayerTo(5, 5);
        
        // Place a bear right next to the player.
        var bearIndex = SpawnTo(4, 5, ElementList.BearId);
        var bear = Actors[bearIndex];
        bear.P1 = 0;
        bear.Cycle = 1;

        // Wait for the bear to attack.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when bear attacks");
        TileAt(4, 5).Id.Should().Be(ElementList.EmptyId,
            "bear should be destroyed");
    }

    [Test]
    public void Bear_ShouldDestroyBreakableWall_WhenPathIsBlockedByBreakable()
    {
        // Place the player.
        MovePlayerTo(8, 5);
        
        // Place a bear.
        var bearIndex = SpawnTo(5, 5, ElementList.BearId);
        var bear = Actors[bearIndex];
        bear.P1 = 0;
        bear.Cycle = 1;
        
        // Put a breakable wall between the bear and the player.
        PlotTo(6, 5, ElementList.BreakableId);

        // Wait for the bear to collide with the breakable wall.
        Step();

        // Assert.
        TileAt(6, 5).Id.Should().Be(ElementList.EmptyId,
            "breakable wall should be destroyed by bear");
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bear should be destroyed by the breakable wall");
    }
}