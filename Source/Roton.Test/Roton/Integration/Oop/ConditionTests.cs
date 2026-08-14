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

        World.Flags.Should().Contain("F1", "F2");
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

        World.Flags.Should().Contain("F2");
    }
}