using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Conditions.Impl;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation.Conditions;

public class EnergizedConditionTests : UnitTestFixture<EnergizedCondition>
{
    [Test]
    [TestCase(1, true)]
    [TestCase(0, false)]
    [TestCase(-1, false)]
    public void Execute_ShouldReturnWhetherEnergyCyclesIsGreaterThanZero(int energyCycles, bool expected)
    {
        // Arrange.
        var world = Mock<IWorld>(mock =>
        {
            mock.Setup(x => x.EnergyCycles)
                .Returns(energyCycles);
        });

        MockService<IEngine>(mock =>
        {
            mock.Setup(x => x.World)
                .Returns(() => world.Object);
        });

        var context = Mock<IOopContext>(_ => { });

        // Act.
        var observed = Subject.Execute(context.Object);

        // Assert.
        observed.Should().Be(expected);
    }
}