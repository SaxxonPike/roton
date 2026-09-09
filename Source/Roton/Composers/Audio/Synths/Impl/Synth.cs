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
    /// Frequency of the output tone, in hz.
    /// </summary>
    private float _frequency;

    /// <summary>
    /// Phase of the output tone, in half-periods.
    /// </summary>
    private float _halfPhase;

    /// <summary>
    /// The amount that the phase should change per sample.
    /// </summary>
    private float _halfPhasePerSample;

    /// <summary>
    /// Output level of the tone.
    /// </summary>
    private float _level = -1;

    /// <summary>
    /// Updates the counter used to determine when the output level should cross
    /// between positive and negative.
    /// </summary>
    private void UpdateFrequency()
    {
        // When the frequency is zeroed out, the phase of the waveform should be reset.

        if (_frequency <= 0)
        {
            _halfPhase = 0;
            _halfPhasePerSample = 0;
        }
        else
        {
            _halfPhasePerSample = Math.Abs(_frequency / config.Audio.SampleRate * 2);
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
    public void Update() =>
        UpdateFrequency();

    /// <inheritdoc />
    public int Render(Span<float> buffer)
    {
        // If the frequency is less than or equal to zero,
        // there is no waveform to render. Returning zero indicates
        // that no samples were populated in the buffer.

        if (_halfPhasePerSample <= 0)
            return 0;

        // For each sample, the half-period count is advanced by an amount
        // determined by the frequency and the configured sample rate.
        // For each time that the half-period crosses 1, the output level
        // phase is inverted.

        var useLowPass = config.Audio.LowPass;
        var useInterpolation = config.Audio.Interpolate;

        for (var idx = 0; idx < buffer.Length; idx++)
        {
            _halfPhase += _halfPhasePerSample;

            while (_halfPhase >= 1)
            {
                _halfPhase -= 1;
                _level = -_level;
            }

            // Audio filters can be applied to shape the square waveform.

            var raw = _level;

            if (useInterpolation)
                raw = _level + _level * synthInterpolator.PolyBlep(_halfPhase, _halfPhasePerSample);

            if (useLowPass)
                raw = synthFilter.LowPass(raw);

            // The output of the filter can exceed +/- 1, so we divide by two to keep
            // the dynamic range intact.

            buffer[idx] = raw / 2;
        }

        return buffer.Length;
    }
}