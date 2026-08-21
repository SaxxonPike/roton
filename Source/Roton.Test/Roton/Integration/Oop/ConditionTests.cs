using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public class ConditionTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void If_ShouldExecuteCurrentLine_WhenConditionIsMet()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked i set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "condition line was not executed");
    }

    [Test]
    public void If_ShouldSkipToNextLine_WhenConditionIsNotMet()
    {
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if not blocked i set f1",
            "#set f2"
        );

        Step();

        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "condition line was not skipped");
    }

    [Test]
    public void Blocked_ShouldEvaluateTrue_WhenBlocked()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked w set f1",
            "#set f2"
        );

        // Place a wall to the west.
        PlotTo(4, 5, Elements.SolidId);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "blocked condition must resolve true when blocked");
    }

    [Test]
    public void Blocked_ShouldEvaluateFalse_WhenNotBlocked()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked w set f1",
            "#set f2"
        );

        // Place a walkable element to the west.
        PlotTo(4, 5, Elements.FakeId);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "blocked condition must resolve false when not blocked");
    }

    [Test]
    public void Blocked_ShouldEvaluateFalse_WithUnknownDirection()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked x set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "blocked condition must resolve false when an unknown direction is specified");
    }

    [Test]
    public void Energized_ShouldEvaluateTrue_WhenPlayerHasEnergyCycles()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if energized set f1",
            "#set f2"
        );

        // Give the player some energy.
        EnergyCycles = 10;

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "energized condition must resolve true when player has energy cycles");
    }

    [Test]
    public void Energized_ShouldEvaluateTrue_WhenPlayerHasNoEnergyCycles()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if energized set f1",
            "#set f2"
        );

        // Make sure the player has no energy.
        EnergyCycles = 0;

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "energized condition must resolve false when player has no energy cycles");
    }

    [Test]
    public void Aligned_ShouldEvaluateTrue_WhenAlignedHorizontally()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the test actor to the west.
        var index = SpawnTo(5, 10, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if alligned set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "aligned condition must resolve true when aligned horizontally with the player");
    }

    [Test]
    public void Aligned_ShouldEvaluateTrue_WhenAlignedVertically()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the test actor to the north.
        var index = SpawnTo(10, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if alligned set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "aligned condition must resolve true when aligned vertically with the player");
    }

    [Test]
    public void Aligned_ShouldEvaluateFalse_WhenNotAligned()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the test actor somewhere that isn't aligned.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if alligned set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "aligned condition must resolve false when not aligned");
    }

    [Test]
    public void Any_ShouldEvaluateTrue_WhenElementIsPresent_WithBackgroundColor()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if any blue key set f1",
            "#set f2"
        );

        // Place a blue key elsewhere.
        PlotTo(10, 10, Elements.KeyId, 0x29);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "any kind+color condition must resolve true when an element of the specified color is present");
    }

    [Test]
    public void Any_ShouldEvaluateTrue_WhenElementIsPresent_WithColor()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if any blue key set f1",
            "#set f2"
        );

        // Place a blue key elsewhere.
        PlotTo(10, 10, Elements.KeyId, 9);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "any kind+color condition must resolve true when an element of the specified color is present");
    }

    [Test]
    public void Any_ShouldEvaluateTrue_WhenElementIsPresent_WithoutColor()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if any key set f1",
            "#set f2"
        );

        // Place a key elsewhere.
        PlotTo(10, 10, Elements.KeyId, 12);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "any kind condition must resolve true when an element is present");
    }

    [Test]
    public void Any_ShouldEvaluateFalse_WhenElementIsAbsent_WithColor()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if any blue key set f1",
            "#set f2"
        );

        // Place a different color key somewhere.
        PlotTo(10, 10, Elements.KeyId, 10);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "any kind+color condition must resolve false when an element of the specified color is not present");
    }

    [Test]
    public void Any_ShouldEvaluateFalse_WhenElementIsAbsent_WithoutColor()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if any key set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "any kind condition must resolve false when an element is not present");
    }

    [Test]
    public void Any_ShouldEvaluateFalse_WithUnknownKind()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if any banana set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "any kind condition must resolve false when an unknown element is specified");
    }

    [Test]
    public void Contact_ShouldEvaluateTrue_WhenAdjacentToPlayer()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the test actor adjacent to the player.
        var index = SpawnTo(9, 10, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if contact set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "contact condition must resolve true when adjacent to the player");
    }

    [Test]
    public void Contact_ShouldEvaluateFalse_WhenNotAdjacentToPlayer()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Place the test actor not adjacent to the player.
        var index = SpawnTo(8, 10, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if contact set f1",
            "#set f2"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "contact condition must resolve false when not adjacent to the player");
    }

    [Test]
    public void Not_ShouldNegateCondition_WhenTrue()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if not blocked w set f1",
            "#set f2"
        );

        // Place a non-blocking element to the west.
        PlotTo(4, 5, Elements.FakeId);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().Contain(["F1"],
            "not condition must resolve true when the condition it negates is false");
    }

    [Test]
    public void Not_ShouldNegateCondition_WhenFalse()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if not blocked w set f1",
            "#set f2"
        );

        // Place a blocking element to the west.
        PlotTo(4, 5, Elements.SolidId);

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F2"],
            "code was not executed");
        Flags.Should().NotContain(["F1"],
            "not condition must resolve false when the condition it negates is true");
    }
}