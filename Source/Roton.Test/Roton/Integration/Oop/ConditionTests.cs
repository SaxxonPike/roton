using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Oop;

public class ConditionTests(Context context) : OopTestFixture(context)
{
    [Test]
    public void If_ShouldExecuteCurrentLine_WhenConditionIsMet()
    {
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked i set f1",
            "#set f2"
        );

        Step();

        World.Flags.Should().Contain(["F1", "F2"]);
    }

    [Test]
    public void If_ShouldSkipToNextLine_WhenConditionIsNotMet()
    {
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if not blocked i set f1",
            "#set f2"
        );

        Step();

        World.Flags.Should().Contain(["F2"]);
    }

    [Test]
    public void Blocked_ShouldEvaluateTrue_WhenBlocked()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked w set f1",
            "#set f2"
        );
        
        // Place a wall to the west.
        PlotTo(4, 5, ElementList.SolidId);

        // Execute.
        Step();

        // Assert.
        World.Flags.Should().Contain(["F1", "F2"]);
    }
    
    [Test]
    public void Blocked_ShouldEvaluateFalse_WhenNotBlocked()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if blocked w set f1",
            "#set f2"
        );
        
        // Place a walkable element to the west.
        PlotTo(4, 5, ElementList.FakeId);

        // Execute.
        Step();

        // Assert.
        World.Flags.Should().Contain(["F2"]);
    }
    
    [Test]
    public void Energized_ShouldEvaluateTrue_WhenPlayerHasEnergyCycles()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if energized set f1",
            "#set f2"
        );
        
        // Give the player some energy.
        World.EnergyCycles = 10;

        // Execute.
        Step();

        // Assert.
        World.Flags.Should().Contain(["F1", "F2"]);
    }
    
    [Test]
    public void Energized_ShouldEvaluateTrue_WhenPlayerHasNoEnergyCycles()
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#if energized set f1",
            "#set f2"
        );
        
        // Make sure the player has no energy.
        World.EnergyCycles = 0;

        // Execute.
        Step();

        // Assert.
        World.Flags.Should().Contain(["F2"]);
    }
}