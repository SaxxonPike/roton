using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Synths.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Synth(
    IConfig config)
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
    /// Previous sample, used for interpolation.
    /// </summary>
    private float _lastSample;

    /// <summary>
    /// Updates the counter used to determine when the output level should cross
    /// between positive and negative.
    /// </summary>
    private void UpdateFrequency() =>
        _halfPhasePerSample = Math.Abs(_frequency / config.AudioSampleRate * 2);

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

        var lastSample = _lastSample;

        // For each sample, the half-period count is advanced by an amount
        // determined by the frequency and the configured sample rate.
        // For each time that the half-period crosses 1, the output level
        // phase is inverted.

        for (var idx = 0; idx < buffer.Length; idx++)
        {
            _halfPhase += _halfPhasePerSample;

            while (_halfPhase >= 1)
            {
                _halfPhase -= 1;
                _level = -_level;
            }

            // Perform interpolation.

            lastSample = buffer[idx] = (_level + lastSample) / 2;
        }

        _lastSample = lastSample;
        return buffer.Length;
    }
}