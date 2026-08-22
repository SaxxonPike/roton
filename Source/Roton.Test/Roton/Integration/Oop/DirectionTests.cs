using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public class DirectionTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void ShortMove_ShouldShowError_WhenDirectionIsInvalid()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "/q"
        );

        // Execute.
        Step();

        // Assert.
        Message.Should().BeEquivalentTo(["ERR: Bad direction"],
            "Error message should be shown");
    }

    [Test]
    public void ShortTryMove_ShouldShowError_WhenDirectionIsInvalid()
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            "?q"
        );

        // Execute.
        Step();

        // Assert.
        Message.Should().BeEquivalentTo(["ERR: Bad direction"],
            "Error message should be shown");
    }

    [Test]
    [TestCase("#go n")]
    [TestCase("#go north")]
    [TestCase("/n")]
    [TestCase("/north")]
    [TestCase("?n")]
    [TestCase("?north")]
    public void MoveNorth_ShouldSucceed(string code)
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index, code);

        // Execute.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "object should not be at previous location");
        TileAt(5, 4).Id.Should().Be(Elements.ObjectId,
            "object should have moved north");
    }

    [Test]
    [TestCase("#go s")]
    [TestCase("#go south")]
    [TestCase("/s")]
    [TestCase("/south")]
    [TestCase("?s")]
    [TestCase("?south")]
    public void MoveSouth_ShouldSucceed(string code)
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index, code);

        // Execute.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "object should not be at previous location");
        TileAt(5, 6).Id.Should().Be(Elements.ObjectId,
            "object should have moved south");
    }

    [Test]
    [TestCase("#go w")]
    [TestCase("#go west")]
    [TestCase("/w")]
    [TestCase("/west")]
    [TestCase("?w")]
    [TestCase("?west")]
    public void MoveWest_ShouldSucceed(string code)
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index, code);

        // Execute.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "object should not be at previous location");
        TileAt(4, 5).Id.Should().Be(Elements.ObjectId,
            "object should have moved west");
    }

    [Test]
    [TestCase("#go e")]
    [TestCase("#go east")]
    [TestCase("/e")]
    [TestCase("/east")]
    [TestCase("?e")]
    [TestCase("?east")]
    public void MoveEast_ShouldSucceed(string code)
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index, code);

        // Execute.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "object should not be at previous location");
        TileAt(6, 5).Id.Should().Be(Elements.ObjectId,
            "object should have moved east");
    }
    
    [Test]
    [TestCase("#go i")]
    [TestCase("#go idle")]
    public void GoIdle_ShouldFail(string code)
    {
        // Place the test actor.
        var index = SpawnTo(5, 5, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index, code, "#set f1");

        // Execute.
        Step();
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.ObjectId,
            "object should stay in place");
        Flags.AsEnumerable().Should().NotContain(["F1"],
            "object should not continue execution after using #go idle");
    }

}