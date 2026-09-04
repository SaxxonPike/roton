using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class StarTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Star_DoesNotMove_WhenP2IsEven()
    {
        // When P1 is even, the star will not move or perform any action
        // besides update its tile.

        // Place the star.
        var starIndex = SpawnTo(5, 5, Elements.StarId);
        var star = Actors[starIndex];
        star.Cycle = 1;
        star.P2 = 8;

        // Wait for the star to process.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.StarId,
            "star should not have left its previous position");
    }

    [Test]
    public void Star_MovesTowardPlayer_WhenP2IsOdd()
    {
        // When P1 is odd, the star will move towards the player.

        // Place the star.
        var starIndex = SpawnTo(5, 5, Elements.StarId);
        var star = Actors[starIndex];
        star.Cycle = 1;
        star.P2 = 9;

        // Place the player.
        MovePlayerTo(10, 5);

        // Wait for the star to process.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "star should have left its previous position");
        TileAt(6, 5).Id.Should().Be(Elements.StarId,
            "star should have moved towards the player");
    }

    [Test]
    public void Star_DestroysBreakable()
    {
        // Stars will destroy breakable tiles.

        // Place the star.
        var starIndex = SpawnTo(5, 5, Elements.StarId);
        var star = Actors[starIndex];
        star.Cycle = 1;
        star.P2 = 9;

        // Place a breakable wall between the star and the player.
        PlotTo(6, 5, Elements.BreakableId);

        // Place the player.
        MovePlayerTo(10, 5);

        // Wait for the star to process.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "star should have left its previous position");
        TileAt(6, 5).Id.Should().Be(Elements.EmptyId,
            "star should have destroyed the breakable wall");
    }

    [Test]
    public void Star_AttacksPlayer()
    {
        // Stars will damage the player.

        // Place the star.
        var starIndex = SpawnTo(5, 5, Elements.StarId);
        var star = Actors[starIndex];
        star.Cycle = 1;
        star.P2 = 9;

        // Place the player next to the star.
        MovePlayerTo(6, 5);

        // Wait for the star to process.
        var initialHealth = Health;
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "star should have left its previous position");
        TileAt(6, 5).Id.Should().Be(Elements.PlayerId,
            "player should remain at the same position");
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should have taken damage");

    }
}