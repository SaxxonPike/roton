using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Emulation.Targets.Impl;

namespace Roton.Test.Roton.Emulation.Targets;

public class OthersTargetTests : TargetTestFixture<OthersTarget>
{
    [Test]
    [TestCase(0, 1, 1, false)] // searchIndex >= actorsCount (0 >= 1 false, but searchIndex=index=0, next searchIndex 1 >= 1)
    [TestCase(0, 0, 2, true)]  // index=0, searchIndex=0, actorsCount=2. searchIndex==index, searchIndex++ (1). 1 < 2. True.
    [TestCase(1, 0, 2, true)]  // index=1, searchIndex=0, actorsCount=2. searchIndex!=index. True.
    public void Execute_ShouldReturnWhetherSearchIndexMatchesTarget(int index, int searchIndex, int actorsCount, bool expected)
    {
        // Arrange.
        var context = Mock<ISearchContext>(mock =>
        {
            mock.SetupAllProperties();
            mock.Object.SearchIndex = searchIndex;
        });

        MockService<IActors>(mock =>
        {
            mock.Setup(x => x.Count)
                .Returns(actorsCount);
        });

        // Act.
        var observed = Subject.Execute(index, context.Object, string.Empty);

        // Assert.
        observed.Should().Be(expected);
    }
}