using System;
#if NET10_0_OR_GREATER
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
#endif
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Impl;

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
#if NET10_0_OR_GREATER
            var idx = 0;

            // Vectorized version of the scalar loop below.

            if (Vector256.IsHardwareAccelerated && idx <= buffer.Length - 8)
            {
                var vDelta = Vector256.Create(_deltaPhase);
                var vOffsets = Vector256.Create(0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f) * vDelta;
                var vHalf = Vector256.Create(0.5f);
                var vOne = Vector256.Create(1.0f);
                var vNegOne = Vector256.Create(-1.0f);

                while (idx <= buffer.Length - 8)
                {
                    var vP = Vector256.Create(_phase) + vOffsets;
                    vP -= Vector256.Floor(vP);
                    vP.StoreUnsafe(ref MemoryMarshal.GetReference(phases), (nuint)idx);

                    var vSquare = Vector256.ConditionalSelect(Vector256.LessThan(vP, vHalf), vOne, vNegOne);
                    vSquare.StoreUnsafe(ref MemoryMarshal.GetReference(buffer), (nuint)idx);

                    var nextBase = _phase + _deltaPhase * 8f;
                    _phase = nextBase - MathF.Truncate(nextBase);
                    idx += 8;
                }
            }

            // This is the same operation as above, but using 128-bit vectors.

            else if (Vector128.IsHardwareAccelerated && idx <= buffer.Length - 4)
            {
                var vDelta = Vector128.Create(_deltaPhase);
                var vOffsets = Vector128.Create(0f, 1f, 2f, 3f) * vDelta;
                var vHalf = Vector128.Create(0.5f);
                var vOne = Vector128.Create(1.0f);
                var vNegOne = Vector128.Create(-1.0f);

                while (idx <= buffer.Length - 4)
                {
                    var vP = Vector128.Create(_phase) + vOffsets;
                    vP = vP - Vector128.Floor(vP);
                    vP.StoreUnsafe(ref MemoryMarshal.GetReference(phases), (nuint)idx);

                    var vSquare = Vector128.ConditionalSelect(Vector128.LessThan(vP, vHalf), vOne, vNegOne);
                    vSquare.StoreUnsafe(ref MemoryMarshal.GetReference(buffer), (nuint)idx);

                    var nextBase = _phase + _deltaPhase * 4f;
                    _phase = nextBase - MathF.Truncate(nextBase);
                    idx += 4;
                }
            }

            // Generation function: a full period is represented in time as 0.0-1.0 (phase)
            // Square wave is generated like so: positive if < 0.5, negative if >= 0.5.

            while (idx < buffer.Length)
            {
                phases[idx] = _phase;
                buffer[idx] = _phase < 0.5f ? 1.0f : -1.0f;
                var nextPhase = _phase + _deltaPhase;
                _phase = nextPhase - MathF.Truncate(nextPhase);
                idx++;
            }
#else
            // Scalar-only version of the square generator.

            for (var idx = 0; idx < buffer.Length; idx++)
            {
                buffer[idx] = _phase < 0.5f ? 1.0f : -1.0f;
                phases[idx] = _phase;
                var nextPhase = _phase + _deltaPhase;
                _phase = nextPhase - (float)Math.Truncate(nextPhase);
            }
#endif
        }

        // Apply post-processing effects.

        if (config.Audio.Interpolate)
            synthInterpolator.Interpolate(buffer, phases, _deltaPhase);

        if (config.Audio.LowPass)
            synthFilter.Filter(buffer);

        return buffer.Length;
    }
}