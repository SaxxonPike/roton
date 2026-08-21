using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public class FlagTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Flags_ShouldBeSetBySetCommand()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f1"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().Contain(["F1"],
            "flag was not set");
    }

    [Test]
    public void Flags_ShouldBeClearedByClearCommand()
    {
        // Set the flag.
        Flags.Add("F1");

        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#clear f1"
        );

        // Execute.
        Step();

        // Assert.
        Flags.Should().NotContain(["F1"],
            "flag was not cleared");
    }

    [Test]
    public void Flags_ShouldNotBeSetTwice()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f1",
            "#set f1"
        );

        // Execute.
        Step();

        // Assert.
        Flags[0].Should().Be("F1");
        Flags[1].Should().BeEmpty();
    }

    [Test]
    public void Flags_ShouldReplaceHighestFlag_WhenFlagsAreFull()
    {
        // Fill the flag list.
        for (var i = 0; i < Flags.Count; i++)
            Flags[i] = $"F{i}";

        var expectedFlags = Enumerable
            .Range(0, Flags.Count - 1)
            .Select(i => Flags[i])
            .Concat(["F99"])
            .ToList();

        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "#set f99"
        );

        // Execute.
        Step();

        // Assert.
        Flags.AsEnumerable().Should().BeEquivalentTo(expectedFlags);
    }
}