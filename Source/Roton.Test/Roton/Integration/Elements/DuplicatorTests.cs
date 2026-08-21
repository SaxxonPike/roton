using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class DuplicatorTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Duplicator_ShouldForceInteraction_WhenTargetIsOccupiedByPlayer()
    {
        // If the player is blocking the duplicator's target tile, the source
        // item will interact with the player.

        // Place the duplicator.
        var duplicatorIndex = SpawnTo(5, 5, Elements.DuplicatorId);
        var duplicator = Actors[duplicatorIndex];
        duplicator.Vector = Vector.East;
        duplicator.P1 = 5;
        duplicator.P2 = 9;
        duplicator.Cycle = 1;

        // Place the player in the duplicator's path.
        MovePlayerTo(4, 5);

        // Place something perishable on the other side.
        PlotTo(6, 5, Elements.GemId);

        // Wait for the duplicator to activate.
        Step();

        // Assert.
        Gems.Should().Be(1,
            "duplicator source item should be interacted with by the blocking player");
        TileAt(6, 5).Id.Should().Be(Elements.EmptyId,
            "the source gem should have been consumed");
    }

    [Test]
    public void Duplicator_ShouldAdvancePhase()
    {
        // Place the duplicator.
        var duplicatorIndex = SpawnTo(5, 5, Elements.DuplicatorId);
        var duplicator = Actors[duplicatorIndex];
        duplicator.Vector = Vector.East;
        duplicator.P1 = 3;
        duplicator.P2 = 9;
        duplicator.Cycle = 1;

        // Wait for the duplicator to advance.
        Step();

        // Assert.
        ((int)duplicator.P1).Should().Be(4,
            "phase (P1) should have advanced 3->4");
    }

    [Test]
    public void Duplicator_ShouldDuplicate_WhenCycleCompletes()
    {
        // Place the duplicator.
        var duplicatorIndex = SpawnTo(5, 5, Elements.DuplicatorId);
        var duplicator = Actors[duplicatorIndex];
        duplicator.Vector = Vector.East;
        duplicator.P1 = 5;
        duplicator.P2 = 9;
        duplicator.Cycle = 1;

        // Place something for it to duplicate.
        PlotTo(6, 5, Elements.GemId, 3);

        // Wait for the duplicator to activate.
        Step();

        // Assert.
        TileAt(4, 5).Id.Should().Be(Elements.GemId,
            "target location should contain a duplicated gem");
        TileAt(4, 5).Color.Should().Be(3,
            "duplicated gem should match source color");
        TileAt(6, 5).Id.Should().Be(Elements.GemId,
            "source gem should remain in place");
        ((int)duplicator.P1).Should().Be(0,
            "phase (P1) should reset to 0");
    }

    [Test]
    public void Duplicator_ShouldDuplicateActor_WhenCycleCompletes()
    {
        // Place the duplicator.
        var duplicatorIndex = SpawnTo(5, 5, Elements.DuplicatorId);
        var duplicator = Actors[duplicatorIndex];
        duplicator.Vector = Vector.East;
        duplicator.P1 = 5;
        duplicator.P2 = 9;
        duplicator.Cycle = 1;

        // Place something for it to duplicate that has stats. Put something
        // in there that isn't normally there to check on the duplicated actor
        // later.
        var sourceActorIndex = SpawnTo(6, 5, Elements.PlayerId);
        Actors[sourceActorIndex].Leader = -123;

        // Wait for the duplicator to activate.
        Step();

        // Assert.
        TileAt(4, 5).Id.Should().Be(Elements.PlayerId,
            "target location should contain a duplicated player");
        ((int)ActorAt(4, 5).Leader).Should().Be(-123,
            "duplicated player should have same stats");
        TileAt(6, 5).Id.Should().Be(Elements.PlayerId,
            "source player should remain in place");
        ((int)duplicator.P1).Should().Be(0,
            "phase (P1) should reset to 0");
    }

    [Test]
    public void Duplicator_ShouldPushAndDuplicate_WhenTargetIsOccupiedByPushable()
    {
        // Place the duplicator.
        var duplicatorIndex = SpawnTo(5, 5, Elements.DuplicatorId);
        var duplicator = Actors[duplicatorIndex];
        duplicator.Vector = Vector.East;
        duplicator.P1 = 5;
        duplicator.P2 = 9;
        duplicator.Cycle = 1;

        // Place something for it to duplicate.
        PlotTo(6, 5, Elements.GemId);

        // Place something pushable in the duplicator's path.
        PlotTo(4, 5, Elements.BoulderId);

        // Wait for the duplicator to activate.
        Step();

        // Assert.
        TileAt(3, 5).Id.Should().Be(Elements.BoulderId,
            "boulder at target location should be pushed away");
        TileAt(4, 5).Id.Should().Be(Elements.GemId,
            "target location should now have the duplicated gem");
        TileAt(6, 5).Id.Should().Be(Elements.GemId,
            "source gem should remain untouched");
    }

    [Test]
    public void Duplicator_ShouldFailToDuplicate_WhenTargetIsBlockedBySolidWall()
    {
        // Place the duplicator.
        var duplicatorIndex = SpawnTo(5, 5, Elements.DuplicatorId);
        var duplicator = Actors[duplicatorIndex];
        duplicator.Vector = Vector.East;
        duplicator.P1 = 5;
        duplicator.P2 = 9;
        duplicator.Cycle = 1;

        // Place something for it to duplicate.
        PlotTo(6, 5, Elements.GemId);

        // Place a solid wall in the duplicator's path.
        PlotTo(4, 5, Elements.SolidId);

        // Wait for the duplicator to activate.
        Step();

        // Assert.
        TileAt(4, 5).Id.Should().Be(Elements.SolidId,
            "wall at target location should remain unaffected");
        TileAt(6, 5).Id.Should().Be(Elements.GemId,
            "source item should remain untouched");
        ((int)duplicator.P1).Should().Be(0,
            "duplicator phase should reset even on failure");
    }
}