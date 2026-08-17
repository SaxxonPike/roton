using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Oop;

public class FlagTests(Context context) : OopTestFixture(context)
{
    [Test]
    public void Flags_ShouldBeSetBySetCommand()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f1"
        );

        // Execute.
        Step();

        // Assert.
        World.Flags.Should().Contain(["F1"],
            "flag was not set");
    }

    [Test]
    public void Flags_ShouldBeClearedByClearCommand()
    {
        // Set the flag.
        World.Flags.Add("F1");

        // Place the test actor.
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#clear f1"
        );

        // Execute.
        Step();

        // Assert.
        World.Flags.Should().NotContain(["F1"],
            "flag was not cleared");
    }

    [Test]
    public void Flags_ShouldNotBeSetTwice()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f1",
            "#set f1"
        );

        // Execute.
        Step();

        // Assert.
        World.Flags[0].Should().Be("F1");
        World.Flags[1].Should().BeEmpty();
    }

    [Test]
    public void Flags_ShouldReplaceHighestFlag_WhenFlagsAreFull()
    {
        // Fill the flag list.
        for (var i = 0; i < World.Flags.Count; i++)
            World.Flags[i] = $"F{i}";

        var expectedFlags = Enumerable
            .Range(0, World.Flags.Count - 1)
            .Select(i => World.Flags[i])
            .Concat(["F99"])
            .ToList();

        // Place the test actor.
        var index = SpawnTo(1, 1, ElementList.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f99"
        );

        // Execute.
        Step();

        // Assert.
        World.Flags.AsEnumerable().Should().BeEquivalentTo(expectedFlags);
    }
}