using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Conditions.Impl;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Test.Roton.Emulation.Conditions;

public class AnyConditionTests : ConditionTestFixture<AnyCondition>
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void Execute_ShouldReturnWhetherTileWasFound(bool findResult)
    {
        // Arrange.
        var kind = Mock<ITile>(_ => { }).Object;
        var parser = Mock<IParser>(mock =>
        {
            mock.Setup(x => x.GetKind(It.IsAny<IOopContext>()))
                .Returns(kind);
        });

        MockService<IEngine>(mock =>
        {
            mock.Setup(x => x.Parser)
                .Returns(() => parser.Object);
            mock.Setup(x => x.FindTile(kind, It.Is<IXyPair>(p => p.X == 0 && p.Y == 1)))
                .Returns(findResult);
        });

        var context = Mock<IOopContext>(_ => { });

        // Act.
        var observed = Subject.Execute(context.Object);

        // Assert.
        observed.Should().Be(findResult);
    }

    [Test]
    public void Execute_ShouldReturnNullIfKindIsNull()
    {
        // Arrange.
        var parser = Mock<IParser>(mock =>
        {
            mock.Setup(x => x.GetKind(It.IsAny<IOopContext>()))
                .Returns((ITile)null);
        });

        MockService<IEngine>(mock =>
        {
            mock.Setup(x => x.Parser)
                .Returns(() => parser.Object);
        });

        var context = Mock<IOopContext>(_ => { });

        // Act.
        var observed = Subject.Execute(context.Object);

        // Assert.
        observed.Should().BeNull();
    }
}