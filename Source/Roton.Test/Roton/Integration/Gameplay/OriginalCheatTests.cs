using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Gameplay;

public class OriginalCheatTests : OriginalContextTestFixture
{
    [Test]
    public void TorchesCheat_ShouldGiveTorches()
    {
        TypeCheat("torches");
        Torches.Should().Be(3,
            "player should have gained torches");
    }

    [Test]
    public void DarkCheat_ShouldMakeBoardDark()
    {
        TypeCheat("dark");
        IsDark.Should().BeTrue(
            "board should be dark");
    }
    
    [Test]
    public void MinusDarkCheat_ShouldMakeBoardNotDark()
    {
        IsDark = true;
        TypeCheat("-dark");
        IsDark.Should().BeFalse(
            "board should not be dark");
    }
}