using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class RuffianTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Ruffian_ShouldMoveTowardsPlayer_WhenAlignedAndActive()
    {
        // While moving, ruffians have a chance to immediately bolt for the player
        // if aligned depending on their intelligence stat (P1).

        // Place the player.
        MovePlayerTo(10, 5);

        // Place the ruffian.
        var ruffianIndex = SpawnTo(5, 5, ElementList.RuffianId);
        var ruffian = Actors[ruffianIndex];
        ruffian.Vector = Vector.East;

        // Ordinarily you can't set P2 to 10 in the editor, but we want to
        // effectively disable resting for this test.
        ruffian.P1 = 9;
        ruffian.P2 = 10;
        ruffian.Cycle = 1;

        // Wait for the ruffian to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "ruffian should vacate initial position");
        TileAt(6, 5).Id.Should().Be(ElementList.RuffianId,
            "ruffian should advance forward along vector");
    }

    [Test]
    public void Ruffian_ShouldAttackPlayer_WhenAdjacent()
    {
        // Place the player.
        MovePlayerTo(6, 5);

        // Place the ruffian directly next to the player.
        var ruffianIndex = SpawnTo(5, 5, ElementList.RuffianId);
        var ruffian = Actors[ruffianIndex];
        ruffian.Vector = Vector.East;
        ruffian.P1 = 9;
        ruffian.P2 = 10;
        ruffian.Cycle = 1;

        // Wait for the ruffian to activate.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when ruffian attacks");
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "ruffian should be removed after attacking player");
    }

    [Test]
    public void Ruffian_ShouldStopMovement_WhenHittingSolidWall()
    {
        // Ruffians switch to rest state once they collide with something solid
        // or by random chance determined by resting rate stat (P2).

        // Place the ruffian and make it hurry east.
        var ruffianIndex = SpawnTo(5, 5, ElementList.RuffianId);
        var ruffian = Actors[ruffianIndex];
        ruffian.Vector = Vector.East;
        ruffian.P1 = 9;
        ruffian.P2 = 10;
        ruffian.Cycle = 1;

        // Place a wall in front of the ruffian.
        PlotTo(6, 5, ElementList.SolidId);

        // Wait for the ruffian to activate.
        Step();

        TileAt(5, 5).Id.Should().Be(ElementList.RuffianId,
            "ruffian should remain at initial tile when blocked");
        TileAt(6, 5).Id.Should().Be(ElementList.SolidId,
            "wall should remain");
        ruffian.Vector.IsZero().Should().BeTrue(
            "ruffian should enter rest state");
    }

    [Test]
    public void Ruffian_ShouldDamagePlayer_WhenPlayerTouchesRuffian()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the ruffian.
        SpawnTo(4, 3, ElementList.RuffianId);

        // Move the player into the ruffian.
        var initialHealth = Health;
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when walking into ruffian");
        TileAt(3, 3).Id.Should().Be(ElementList.EmptyId,
            "player should move from original location");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should occupy ruffian tile after destroying it");
    }
}