using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Targets.Impl;

namespace Roton.Test.Roton.Emulation.Targets;

public class DefaultTargetTests : TargetTestFixture<DefaultTarget>
{
    [Test]
    public void Execute_ShouldReturnTrueIfActorNameMatchesTerm()
    {
        // Arrange.
        var term = "TARGET";
        var actor = Mock<IActor>(mock =>
        {
            mock.Setup(x => x.Pointer).Returns(1);
        });

        var context = Mock<ISearchContext>(mock =>
        {
            mock.SetupAllProperties();
            mock.Object.SearchIndex = 0;
        });

        MockService<IActors>(mock =>
        {
            mock.Setup(x => x.Count).Returns(1);
            mock.Setup(x => x[0]).Returns(actor.Object);
        });

        MockService<IParser>(mock =>
        {
            mock.Setup(x => x.ReadByte(0, It.IsAny<IExecutable>())).Returns(0x40);
            mock.Setup(x => x.ReadWord(0, It.IsAny<IExecutable>())).Returns(term);
        });

        // Act.
        var observed = Subject.Execute(0, context.Object, term);

        // Assert.
        observed.Should().BeTrue();
    }

    [Test]
    public void Execute_ShouldReturnFalseIfNoActorMatches()
    {
        // Arrange.
        var context = Mock<ISearchContext>(mock =>
        {
            mock.SetupAllProperties();
            mock.Object.SearchIndex = 0;
        });

        MockService<IActors>(mock =>
        {
            mock.Setup(x => x.Count).Returns(0);
        });

        // Act.
        var observed = Subject.Execute(0, context.Object, "MISSING");

        // Assert.
        observed.Should().BeFalse();
    }
}