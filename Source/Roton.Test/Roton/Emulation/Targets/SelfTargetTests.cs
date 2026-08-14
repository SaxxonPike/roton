using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Emulation.Targets.Impl;

namespace Roton.Test.Roton.Emulation.Targets;

public class SelfTargetTests : TargetTestFixture<SelfTarget>
{
    [Test]
    [TestCase(1, 0, true, 1)]
    [TestCase(1, 1, true, 1)]
    [TestCase(1, 2, false, 2)]
    [TestCase(0, 0, false, 0)]
    public void Execute_ShouldReturnWhetherIndexIsSelf(int index, int searchIndex, bool expected, int expectedSearchIndex)
    {
        // Arrange.
        var context = Mock<ISearchContext>(mock =>
        {
            mock.SetupAllProperties();
            mock.Object.SearchIndex = searchIndex;
        });

        // Act.
        var observed = Subject.Execute(index, context.Object, string.Empty);

        // Assert.
        observed.Should().Be(expected);
        context.Object.SearchIndex.Should().Be(expectedSearchIndex);
    }
}