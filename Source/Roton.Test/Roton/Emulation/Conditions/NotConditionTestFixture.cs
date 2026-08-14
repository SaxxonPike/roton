using FluentAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Conditions.Impl;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation.Conditions;

public class NotConditionTestFixture : UnitTestFixture<NotCondition>
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void Execute_ShouldReturnInvertedInnerResult(bool innerResult)
    {
        // Arrange.
        var parser = Mock<IParser>(mock =>
        {
            mock.Setup(x => x.GetCondition(It.IsAny<IOopContext>()))
                .Returns(innerResult);
        });
            
        var actor = Mock<IActor>(_ =>
        {
        });
            
        var context = Mock<IOopContext>(mock =>
        {
            mock.Setup(x => x.Actor)
                .Returns(() => actor.Object);
        });

        MockService<IEngine>(mock =>
        {
            mock.Setup(x => x.Parser)
                .Returns(() => parser.Object);
        });

        // Act.
        var observed = Subject.Execute(context.Object);
            
        // Assert.
        var expected = !innerResult;
        observed.Should().Be(expected);
    }
}