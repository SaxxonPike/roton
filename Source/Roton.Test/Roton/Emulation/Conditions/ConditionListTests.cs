using System;
using System.Collections.Generic;
using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Conditions;
using Roton.Emulation.Conditions.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Test.Roton.Emulation.Conditions;

public class ConditionListTests : ConditionTestFixture<ConditionList>
{
    [Test]
    public void Get_ShouldReturnConditionFromMetadata()
    {
        // Arrange.
        var condition = Mock<ICondition>(_ => { }).Object;
        var metadata = new ContextAttribute(Context.Original, "TEST");
        
        var metadataService = Mock<IContextMetadataService>(mock =>
        {
            mock.Setup(x => x.GetMetadata(condition))
                .Returns([metadata]);
        });

        var conditions = new[] { condition };

        var subject = new ConditionList(
            new Lazy<IContextMetadataService>(() => metadataService.Object),
            new Lazy<IEnumerable<ICondition>>(() => conditions));

        // Act.
        var observed = subject.Get("TEST");

        // Assert.
        observed.Should().Be(condition);
    }

    [Test]
    public void Get_ShouldReturnNullIfConditionNotFound()
    {
        // Arrange.
        var metadataService = Mock<IContextMetadataService>(mock =>
        {
            mock.Setup(x => x.GetMetadata(It.IsAny<object>()))
                .Returns([]);
        });

        var subject = new ConditionList(
            new Lazy<IContextMetadataService>(() => metadataService.Object),
            new Lazy<IEnumerable<ICondition>>(Array.Empty<ICondition>));

        // Act.
        var observed = subject.Get("MISSING");

        // Assert.
        observed.Should().BeNull();
    }
}