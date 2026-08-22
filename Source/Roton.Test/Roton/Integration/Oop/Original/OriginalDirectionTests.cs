using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop.Original;

public class OriginalDirectionTests : OriginalContextTestFixture
{
    [Test]
    [TestCase("/i")]
    [TestCase("/idle")]
    [TestCase("?i")]
    [TestCase("?idle")]
    public void ShortMoveIdle_ShouldSucceed(string code)
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
        Flags.AsEnumerable().Should().Contain(["F1"],
            "object should continue execution after moving idle");
    }

}