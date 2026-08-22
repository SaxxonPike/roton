using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class LionTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Lion_ShouldSeekPlayer()
    {
        // A lion has a random chance to approach the player based on
        // its intelligence rating (P1).

        // Place the player.
        MovePlayerTo(5, 8);

        // Place the lion near the player, but not adjacent.
        var lionIndex = SpawnTo(5, 5, Elements.LionId);
        var lion = Actors[lionIndex];
        lion.P1 = 9;
        lion.Cycle = 1;

        // Wait for the lion to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "lion should have left its tile");
        TileAt(5, 6).Id.Should().Be(Elements.LionId,
            "lion should have moved toward the player");
    }

    [Test]
    public void Lion_ShouldAttackPlayer_WhenAdjacent()
    {
        // Place the player.
        MovePlayerTo(5, 5);

        // Place the lion adjacent to the player.
        var lionIndex = SpawnTo(5, 4, Elements.LionId);
        var lion = Actors[lionIndex];
        lion.P1 = 9;
        lion.Cycle = 1;

        // Wait for the lion to move.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when lion attacks");
        TileAt(5, 4).Id.Should().Be(Elements.EmptyId,
            "lion should be destroyed");
    }

    [Test]
    public void Lion_ShouldDamagePlayer_WhenPlayerTouchesLion()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the lion adjacent to the player.
        SpawnTo(4, 3, Elements.LionId);

        // Move the player into the lion.
        var initialHealth = Health;
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when walking into lion");
        TileAt(3, 3).Id.Should().Be(Elements.EmptyId,
            "player should have left its original location");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should have moved to the lion's location");
    }
}