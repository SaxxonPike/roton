using System;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class SynthFilter : ISynthFilter
{
    /// <summary>
    /// Smoothing coefficient for the two-pole low-pass filter, derived from the
    /// configured cutoff frequency and sample rate.
    /// </summary>
    private float _alpha;

    /// <summary>
    /// Previous output of the first filter stage, used for the two-pole low-pass filter.
    /// </summary>
    private float _filterState;

    /// <summary>
    /// Previous output of the second filter stage, used for the two-pole low-pass filter.
    /// </summary>
    private float _filterState2;

    /// <inheritdoc />
    public void Update(float cutoff, float sampleRate) =>
        _alpha = 1f - (float)Math.Exp(-2.0 * Math.PI * cutoff / sampleRate);

    /// <inheritdoc />
    public void Filter(Span<float> buffer)
    {
        for (var i = 0; i < buffer.Length; i++)
        {
            _filterState += _alpha * (buffer[i] - _filterState);
            _filterState2 += _alpha * (_filterState - _filterState2);
            buffer[i] = _filterState2;
        }
    }
}