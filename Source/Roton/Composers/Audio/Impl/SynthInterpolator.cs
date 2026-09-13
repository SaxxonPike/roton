using System;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class SynthInterpolator : ISynthInterpolator
{
    /// <inheritdoc />
    public void Interpolate(Span<float> buffer, ReadOnlySpan<float> phases, float deltaPhase)
    {
        for (var idx = 0; idx < buffer.Length; idx++)
        {
            var t = phases[idx];
            float y;

            if (t < deltaPhase)
            {
                t /= deltaPhase;
                y = t + t - t * t - 1f;
            }
            else if (t > 1f - deltaPhase)
            {
                t = (t - 1f) / deltaPhase;
                y = t * t + t + t + 1f;
            }
            else
            {
                continue;
            }

            buffer[idx] += y;
        }
    }
}