using FluentAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Conditions.Impl;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation.Conditions
{
    public class BlockedConditionTestFixture : UnitTestFixture<BlockedCondition>
    {
        [Test]
        [TestCase(false, false, null)]
        [TestCase(false, true, null)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        public void Execute_ShouldReturnWhetherTargetTileIsNonFloor(bool hasDirection, bool targetIsFloor,
            bool? expected)
        {
            // Arrange.
            var direction = hasDirection
                ? Mock<IXyPair>(mock =>
                {
                    mock.Setup(m => m.X).Returns(Create<int>());
                    mock.Setup(m => m.Y).Returns(Create<int>());
                })
                : null;

            var actorLocation = new Location(1, 1);

            var actor = Mock<IActor>(mock =>
            {
                mock.Setup(x => x.Location)
                    .Returns(actorLocation);
            });

            var context = Mock<IOopContext>(mock =>
            {
                mock.Setup(x => x.Actor)
                    .Returns(() => actor.Object);
            });

            var element = Mock<IElement>(mock =>
            {
                mock.Setup(x => x.IsFloor)
                    .Returns(targetIsFloor);
            });

            var parser = Mock<IParser>(mock =>
            {
                mock.Setup(x => x.GetDirection(It.IsAny<IOopContext>()))
                    .Returns(direction?.Object);
            });

            var engine = MockService<IEngine>(mock =>
            {
                mock.Setup(x => x.ElementAt(It.IsAny<IXyPair>()))
                    .Returns(element.Object);
                mock.Setup(x => x.Parser)
                    .Returns(parser.Object);
            });

            // Act.
            var observed = Subject.Execute(context.Object);

            // Assert.
            observed.Should().Be(expected, "result should reflect target tile's non-blocking status");
            if (hasDirection)
                engine.Verify(x => x.ElementAt(It.Is<IXyPair>(p => p.Matches(actorLocation.Sum(direction.Object)))),
                    "correct direction needs to be checked");
        }
    }
}