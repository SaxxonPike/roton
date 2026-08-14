using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Emulation.Targets.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation.Targets;

public class AllTargetTests : UnitTestFixture<AllTarget>
{
    [Test]
    [TestCase(0, 1, true)]
    [TestCase(1, 1, false)]
    [TestCase(2, 1, false)]
    public void Execute_ShouldReturnWhetherSearchIndexIsLessThanActorsCount(int searchIndex, int actorsCount, bool expected)
    {
        // Arrange.
        var context = Mock<ISearchContext>(mock =>
        {
            mock.SetupAllProperties();
            mock.Object.SearchIndex = searchIndex;
        });

        var actors = Mock<IActors>(mock =>
        {
            mock.Setup(x => x.Count)
                .Returns(actorsCount);
        });

        MockService<IActors>(mock =>
        {
            mock.Setup(x => x.Count)
                .Returns(actorsCount);
        });

        // Act.
        var observed = Subject.Execute(0, context.Object, string.Empty);

        // Assert.
        observed.Should().Be(expected);
    }
}