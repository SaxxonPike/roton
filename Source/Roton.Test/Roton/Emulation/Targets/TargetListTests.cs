using System;
using System.Collections.Generic;
using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Targets;
using Roton.Emulation.Targets.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Test.Roton.Emulation.Targets;

public class TargetListTests : TargetTestFixture<TargetList>
{
    [Test]
    public void Get_ShouldReturnTargetFromMetadata()
    {
        // Arrange.
        var target = Mock<ITarget>(_ => { }).Object;
        var metadata = new ContextAttribute(Context.Original, "TEST");

        var metadataService = Mock<IContextMetadataService>(mock =>
        {
            mock.Setup(x => x.GetMetadata(target))
                .Returns([metadata]);
        });

        var targets = new[] { target };

        var subject = new TargetList(
            new Lazy<IContextMetadataService>(() => metadataService.Object),
            new Lazy<IEnumerable<ITarget>>(() => targets));

        // Act.
        var observed = subject.Get("TEST");

        // Assert.
        observed.Should().Be(target);
    }

    [Test]
    public void Get_ShouldReturnNullIfTargetNotFound()
    {
        // Arrange.
        var metadataService = Mock<IContextMetadataService>(mock =>
        {
            mock.Setup(x => x.GetMetadata(It.IsAny<object>()))
                .Returns(Array.Empty<ContextAttribute>());
        });

        var subject = new TargetList(
            new Lazy<IContextMetadataService>(() => metadataService.Object),
            new Lazy<IEnumerable<ITarget>>(Array.Empty<ITarget>));

        // Act.
        var observed = subject.Get("MISSING");

        // Assert.
        observed.Should().BeNull();
    }
}