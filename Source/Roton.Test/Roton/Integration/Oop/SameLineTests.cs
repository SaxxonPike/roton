using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Oop;

public class SameLineTests(Context context) : OopTestFixture(context)
{
    [Test]
    public void ShortMovement_ShouldRunCommandsOnSameLine()
    {
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "/i#set f2"
        );

        Step(2);

        Flags.Should().Contain("F2");
    }

    [Test]
    public void ShortMovement_ShouldRunCommandsOnSameLine_WhenPrecededByIf()
    {
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f1",
            "#if f1 /i#set f2"
        );

        Step(2);

        Flags.Should().Contain("F2");
    }
}