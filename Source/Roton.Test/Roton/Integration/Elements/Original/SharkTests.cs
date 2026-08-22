using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements.Original;

public class SharkTests : OriginalContextTestFixture
{
    [Test]
    public void Shark_ShouldMoveOnWaterTowardsPlayer_WhenSeeking()
    {
        // Sharks can only move on water.

        // Place the player.
        MovePlayerTo(10, 5);
        
        // Make the fish tank.
        PlotTo(5, 5, Elements.WaterId);
        PlotTo(6, 5, Elements.WaterId);
        PlotTo(7, 5, Elements.WaterId);

        // Place the shark in it.
        var sharkIndex = SpawnTo(5, 5, Elements.SharkId);
        var shark = Actors[sharkIndex];
        shark.P1 = 10;
        shark.Cycle = 1;

        // Wait for the shark to activate.
        Step();

        TileAt(5, 5).Id.Should().Be(Elements.WaterId,
            "shark should leave water tile behind when moving");
        TileAt(6, 5).Id.Should().Be(Elements.SharkId,
            "shark should advance along water towards player");
    }

    [Test]
    public void Shark_ShouldNotMoveOntoEmptyFloor()
    {
        // Place the player.
        MovePlayerTo(10, 5);
        var sharkIndex = SpawnTo(5, 5, Elements.SharkId);
        var shark = Actors[sharkIndex];
        shark.P1 = 10;
        shark.Cycle = 1;

        // Wait for the shark to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.SharkId,
            "shark should not move onto non-water element");
    }

    [Test]
    public void Shark_ShouldAttackPlayer_WhenAdjacent()
    {
        // Place the player.
        MovePlayerTo(6, 5);
        
        // Place the shark.
        var sharkIndex = SpawnTo(5, 5, Elements.SharkId);
        var shark = Actors[sharkIndex];
        shark.P1 = 10;
        shark.Cycle = 1;

        // Wait for the shark to attack.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when shark attacks");
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "shark should be removed after attacking player");
    }
}
