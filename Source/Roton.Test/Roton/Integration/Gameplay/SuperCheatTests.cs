using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Gameplay;

public class SuperCheatTests : SuperContextTestFixture
{
    [Test]
    public void Z_ShouldGiveZ()
    {
        // Since the default value for Z is -1, the first
        // run of this cheat will set the counter to 0.

        TypeCheat("z");
        Stones.Should().Be(0,
            "player should have gained z");
    }

    [Test]
    public void NoZ_ShouldRemoveZ()
    {
        Stones = 100;
        TypeCheat("noz");
        Stones.Should().Be(-1,
            "player should have all z removed");
    }
}