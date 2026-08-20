using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class BombTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Bomb_ShouldBecomeLit_WhenTouchedByPlayer_AndUnlit()
    {
        // Unlit bombs don't permit the player to push them, they only become lit.

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the bomb.
        var bombIndex = SpawnTo(4, 3, ElementList.BombId);
        var bomb = Actors[bombIndex];
        bomb.P1 = 0;

        // Move the player into the bomb.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        ((int)bomb.P1).Should().Be(Facts.BombCountdownStart - 1,
            "bomb should start countdown and decrement on first cycle");
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
            "player should remain in place");
        TileAt(4, 3).Id.Should().Be(ElementList.BombId,
            "bomb should remain at its location");
    }

    [Test]
    public void Bomb_ShouldBePushed_WhenAlreadyLit()
    {
        // Lit bombs can be pushed.

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the bomb and light it.
        var bombIndex = SpawnTo(4, 3, ElementList.BombId);
        Actors[bombIndex].P1 = 5;

        // Move the player into the bomb.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should push the bomb");
        TileAt(5, 3).Id.Should().Be(ElementList.BombId,
            "bomb should have been pushed");
    }

    [Test]
    public void Bomb_ShouldExplodeAndDestroyBreakables_WhenCountdownEnds()
    {
        // Bombs explode within a radius using breakable tiles.

        // Place a bomb that will explode on the next tick.
        var bombIndex = SpawnTo(5, 5, ElementList.BombId);
        var bomb = Actors[bombIndex];
        bomb.P1 = 2;
        bomb.Cycle = 1;

        // Place both a breakable and solid wall on either side.
        PlotTo(6, 5, ElementList.BreakableId);
        PlotTo(4, 5, ElementList.SolidId);

        // Place a gem that will be destroyed by the explosion.
        PlotTo(5, 6, ElementList.GemId);

        // Wait for the bomb to explode.
        Step();

        // Assert.
        ((int)bomb.P1).Should().Be(1,
            "countdown should decrease to 1");
        TileAt(5, 6).Id.Should().Be(ElementList.EmptyId,
            "breakable tiles should be destroyed");
        TileAt(6, 5).Id.Should().Be(ElementList.BreakableId,
            "breakable walls should remain breakable walls");
        TileAt(4, 5).Id.Should().Be(ElementList.SolidId,
            "non-breakable walls should not change");
    }

    [Test]
    public void Bomb_ShouldClear_AfterCountdownEnds()
    {
        // Bombs clear out all breakable walls within the blast radius after the explosion.

        // Place a bomb that will clean up on the next tick.
        var bombIndex = SpawnTo(5, 5, ElementList.BombId);
        var bomb = Actors[bombIndex];
        bomb.P1 = 1;
        bomb.Cycle = 1;

        // Place both a breakable and solid wall on either side.
        PlotTo(6, 5, ElementList.BreakableId);
        PlotTo(4, 5, ElementList.SolidId);

        // Wait for the bomb to clean up.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bomb should have been destroyed");
        TileAt(6, 5).Id.Should().Be(ElementList.EmptyId,
            "breakable wall should have been removed");
        TileAt(4, 5).Id.Should().Be(ElementList.SolidId,
            "non-breakable walls should not change");
    }
}