using AwesomeAssertions;
using NUnit.Framework;

namespace Roton.Test.Roton.Integration.Elements;

public class TigerTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Tiger_ShouldFireBulletTowardsPlayer_WhenAlignedVertically()
    {
        // Place the player.
        MovePlayerTo(5, 10);
        
        // Place the tiger.
        var tigerIndex = SpawnTo(5, 5, ElementList.TigerId);
        var tiger = Actors[tigerIndex];
        tiger.P1 = 0;
        
        // Ordinarily we can't set P2 above 9 in the editor, but
        // we want to guarantee that the tiger will fire.
        tiger.P2 = 0x7F;
        tiger.Cycle = 1;

        // Wait for the tiger to activate.
        Step();

        // Assert.
        TileAt(5, 6).Id.Should().Be(ElementList.EmptyId,
            "bullet should travel one extra tile");
        TileAt(5, 7).Id.Should().Be(ElementList.BulletId,
            "tiger should fire a bullet toward player");
    }

    [Test]
    public void Tiger_ShouldFireBulletTowardsPlayer_WhenAlignedHorizontally()
    {
        MovePlayerTo(10, 5);
        var tigerIndex = SpawnTo(5, 5, ElementList.TigerId);
        var tiger = Actors[tigerIndex];
        tiger.P1 = 0;
        
        // Ordinarily we can't set P2 above 9 in the editor, but
        // we want to guarantee that the tiger will fire.
        tiger.P2 = 0x7F;
        tiger.Cycle = 1;

        Step();

        TileAt(6, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should travel one extra tile");
        TileAt(7, 5).Id.Should().Be(ElementList.BulletId,
            "tiger should fire a bullet toward player");
    }

    [Test]
    public void Tiger_ShouldFireStarTowardsPlayer_WhenP2HighBitSet()
    {
        // Place the player.
        MovePlayerTo(5, 10);
        
        // Place the tiger, setting it to shoot stars.
        var tigerIndex = SpawnTo(5, 5, ElementList.TigerId);
        var tiger = Actors[tigerIndex];
        tiger.P1 = 0;
        tiger.P2 = 0x80 | 0x7F;
        tiger.Cycle = 1;

        // Wait for the tiger to activate.
        Step();

        // Assert.
        TileAt(5, 6).Id.Should().Be(ElementList.StarId,
            "tiger should fire a star toward player");
    }

    [Test]
    public void Tiger_ShouldAttackPlayer_WhenAdjacent()
    {
        // Place the player.
        MovePlayerTo(5, 5);
        
        // Place the tiger, but prevent it from firing.
        var tigerIndex = SpawnTo(5, 4, ElementList.TigerId);
        var tiger = Actors[tigerIndex];
        tiger.P1 = 10;
        tiger.P2 = 0;
        tiger.Cycle = 1;

        // Wait for the tiger to activate.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when tiger attacks");
        TileAt(5, 4).Id.Should().Be(ElementList.EmptyId,
            "tiger should be destroyed after attacking player");
    }
}
