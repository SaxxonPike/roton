using System;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Synths.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Synth(
    IConfig config,
    ISynthFilter synthFilter,
    ISynthInterpolator synthInterpolator)
    : ISynth
{
    /// <summary>
    /// The constant 2*Pi.
    /// </summary>
    private const float Tau = (float)(Math.PI * 2);

    /// <summary>
    /// Frequency of the output tone, in hz.
    /// </summary>
    private float _frequency;

    /// <summary>
    /// Phase of the output tone (1 period = 1.0).
    /// </summary>
    private float _phase;

    /// <summary>
    /// The amount that the phase should change per sample.
    /// </summary>
    private float _deltaPhase;

    /// <summary>
    /// Updates the counter used to determine when the output level should cross
    /// between positive and negative.
    /// </summary>
    private void UpdateFrequency()
    {
        // When the frequency is zeroed out, the phase of the waveform should be reset.

        if (_frequency <= 0)
        {
            _phase = 0;
            _deltaPhase = 0;
        }
        else
        {
            _deltaPhase = _frequency / config.Audio.SampleRate;
            synthFilter.Update(config.Audio.LowPassCutoff, config.Audio.SampleRate);
        }
    }

    /// <inheritdoc />
    public void SetFrequency(float frequency)
    {
        _frequency = frequency;
        UpdateFrequency();
    }

    /// <inheritdoc />
    public int Render(Span<float> buffer)
    {
        // "Phases" contains the phase for each sample in the buffer.
        // This is primarily useful for interpolation later.

        var phases = (stackalloc float[buffer.Length]);

        // If the frequency is less than or equal to zero,
        // there is no waveform to render. We can take a shortcut
        // by clearing the buffer with no synthesis.

        if (_deltaPhase <= 0)
        {
            buffer.Clear();
        }
        else
        {
            for (var idx = 0; idx < buffer.Length; idx++)
            {
#if NET10_0_OR_GREATER
                buffer[idx] = MathF.Sign(MathF.Sin(_phase * Tau));
                phases[idx] = _phase;
                var nextPhase = _phase + _deltaPhase;
                _phase = nextPhase - MathF.Truncate(nextPhase);
#else
                buffer[idx] = Math.Sign((float)Math.Sin(_phase * Tau));
                phases[idx] = _phase;
                var nextPhase = _phase + _deltaPhase;
                _phase = nextPhase - (float)Math.Truncate(nextPhase);
#endif
            }
        }

        // Apply post-processing effects.

        if (config.Audio.Interpolate)
            synthInterpolator.Interpolate(buffer, phases, _deltaPhase);

        if (config.Audio.LowPass)
            synthFilter.Filter(buffer);

        return buffer.Length;
    }
}