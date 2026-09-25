using System;
using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Composers.Audio;
using Roton.Composers.Audio.Impl;
using Roton.Emulation.Core;
using Roton.Infrastructure;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Composers;

public class SynthTests : BaseTestFixture
{
    [Test]
    public void Render_ShouldGenerateSquareWave_WhenFrequencyIsSet()
    {
        // Arrange.

        var config = Freeze<IConfig>();
        config.SetupGet(c => c.Audio).Returns(new AudioConfig
        {
            SampleRate = 44100,
            Interpolate = false,
            LowPass = false
        });

        var synth = new Synth(
            config.Object,
            Mock.Of<ISynthFilter>(),
            Mock.Of<ISynthInterpolator>()
        );

        synth.SetFrequency(440);
        
        // Act.

        var buffer = new float[64];
        synth.Render(buffer);

        // Assert.

        buffer.Should().Contain(1.0f);
        buffer.Should().Contain(-1.0f);

        foreach (var sample in buffer) 
            sample.Should().BeInRange(-1.0f, 1.0f);
    }

    [Test]
    public void Render_ShouldClearBuffer_WhenFrequencyIsZero()
    {
        // Arrange.

        var config = Freeze<IConfig>();
        config.SetupGet(c => c.Audio).Returns(new AudioConfig
        {
            SampleRate = 44100,
            Interpolate = false,
            LowPass = false
        });

        var synth = new Synth(
            config.Object,
            Mock.Of<ISynthFilter>(),
            Mock.Of<ISynthInterpolator>());

        synth.SetFrequency(0);
        
        // Act.

        var buffer = new float[64];
        buffer.AsSpan().Fill(1.0f);
        synth.Render(buffer);

        // Assert.

        buffer.Should().OnlyContain(x => x == 0);
    }
}
