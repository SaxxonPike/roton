using FluentAssertions;
using NUnit.Framework;
using Roton.Emulation.Conditions.Impl;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation.Conditions;

public class ContactConditionTestFixture : UnitTestFixture<ContactCondition>
{
    [Test]
    [TestCase(0, 0, false)]  // this one's a real shocker, but it is correct
    [TestCase(-1, 0, true)]
    [TestCase(1, 0, true)]
    [TestCase(0, -1, true)]
    [TestCase(0, 1, true)]
    [TestCase(-5, 0, false)]
    [TestCase(5, 0, false)]
    [TestCase(0, -5, false)]
    [TestCase(0, 5, false)]
    [TestCase(-1, -1, false)]
    [TestCase(1, 1, false)]
    public void Execute_ShouldReturnWhetherDistanceIsExactlyOneSquare(int objectXVector, int objectYVector, bool expected)
    {
        // Arrange.
        var playerLocation = Create<Location>();
            
        var player = Mock<IActor>(mock =>
        {
            mock.Setup(x => x.Location)
                .Returns(playerLocation);
        });
            
        var actor = Mock<IActor>(mock =>
        {
            mock.Setup(x => x.Location)
                .Returns(playerLocation.Sum(objectXVector, objectYVector));
        });
            
        var context = Mock<IOopContext>(mock =>
        {
            mock.Setup(x => x.Actor)
                .Returns(() => actor.Object);
        });
            
        MockService<IEngine>(mock =>
        {
            mock.Setup(x => x.Player)
                .Returns(() => player.Object);
        });

        // Act.
        var observed = Subject.Execute(context.Object);
            
        // Assert.
        observed.Should().Be(expected);
    }
}