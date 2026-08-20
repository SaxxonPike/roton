using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;

namespace Roton.Test.Roton.Integration.Elements;

public class RotonTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Roton_ShouldMoveTowardsPlayer_WhenSeeking()
    {
        // Rotons are interesting little creatures that move rapidly either
        // toward the player or clockwise to the player.

        if (ElementList.RotonId < 0)
            Assert.Pass("Roton (the enemy type) does not exist in this context");

        // Place the player.
        MovePlayerTo(10, 5);

        // Place the roton a few spaces away.
        var rotonIndex = SpawnTo(5, 5, ElementList.RotonId);
        var roton = Actors[rotonIndex];

        // Ordinarily you can't set P1 to 10 in the editor,
        // but we want to guarantee a seek.
        roton.P1 = 10;
        roton.Cycle = 1;

        // Wait for the roton to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "roton should leave initial tile when moving");
        TileAt(6, 5).Id.Should().Be(ElementList.RotonId,
            "roton should move horizontally towards player");
    }

    [Test]
    public void Roton_ShouldAttackPlayer_WhenAdjacent()
    {
        if (ElementList.RotonId < 0)
            Assert.Pass("Roton (the enemy type) does not exist in this context");

        // Place the player.
        MovePlayerTo(6, 5);

        // Place the roton adjacent.
        var rotonIndex = SpawnTo(5, 5, ElementList.RotonId);
        var roton = Actors[rotonIndex];
        roton.P1 = 10;
        roton.Cycle = 1;

        // Wait for the roton to attack.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when roton attacks");
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "roton should be removed after attacking player");
    }

    [Test]
    public void Roton_ShouldDamagePlayer_WhenPlayerTouchesRoton()
    {
        if (ElementList.RotonId < 0)
            Assert.Pass("Roton (the enemy type) does not exist in this context");

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the roton adjacent.
        SpawnTo(4, 3, ElementList.RotonId);

        // Move the player into the roton.
        var initialHealth = Health;

        Type(AnsiKey.Right);
        StepAllKeys();

        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when walking into roton");
        TileAt(3, 3).Id.Should().Be(ElementList.EmptyId,
            "player should move from original location");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should occupy roton tile after destroying it");
    }
}