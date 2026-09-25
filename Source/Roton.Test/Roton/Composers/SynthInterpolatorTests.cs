using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Composers.Audio.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Composers;

public class SynthInterpolatorTests : BaseTestFixture
{
    [Test]
    public void Interpolate_ShouldSmoothTransition_WhenPhasesAreNearBoundaries()
    {
        var interpolator = Create<SynthInterpolator>();
        var buffer = new float[16];
        var phases = new float[16];
        const float deltaPhase = 0.1f;

        // Arrange: input waveform.

        for (var i = 0; i < 16; i++)
        {
            phases[i] = i * 0.08f % 1.0f;
            buffer[i] = phases[i] < 0.5f ? 1.0f : -1.0f;
        }

        // Arrange: scalar calculation of BLEP interpolation.

        var scalarBuffer = buffer.ToArray();

        for (var idx = 0; idx < scalarBuffer.Length; idx++)
        {
            var t = phases[idx];

            switch (t)
            {
                case < deltaPhase:
                    t /= deltaPhase;
                    scalarBuffer[idx] += t + t - t * t - 1f;
                    break;
                case > 1f - deltaPhase:
                    t = (t - 1f) / deltaPhase;
                    scalarBuffer[idx] += t * t + t + t + 1f;
                    break;
            }
        }

        // Act.

        interpolator.Interpolate(buffer, phases, deltaPhase);

        // Assert.

        for (var i = 0; i < buffer.Length; i++)
            buffer[i].Should().BeApproximately(scalarBuffer[i], float.Epsilon);
    }
}