using System;
#if NET10_0_OR_GREATER
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
#endif
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
#if NET10_0_OR_GREATER
        var idx = 0;
        var invDeltaPhase = 1f / deltaPhase;
        var vDeltaPhase = Vector256.Create(deltaPhase);
        var vInvDeltaPhase = Vector256.Create(invDeltaPhase);
        var vOneMinusDelta = Vector256.Create(1f - deltaPhase);
        var vOne = Vector256.Create(1f);
        var vTwo = Vector256.Create(2f);
        var vZero = Vector256.Create(0f);
        
        // Vectorized BLEP interpolation.

        if (Vector256.IsHardwareAccelerated)
        {
            while (idx <= buffer.Length - 8)
            {
                var vT = Vector256.LoadUnsafe(ref MemoryMarshal.GetReference(phases), (nuint)idx);

                var mask1 = Vector256.LessThan(vT, vDeltaPhase);
                var t1 = vT * vInvDeltaPhase;
                var y1 = vTwo * t1 - t1 * t1 - vOne;

                var mask2 = Vector256.GreaterThan(vT, vOneMinusDelta);
                var t2 = (vT - vOne) * vInvDeltaPhase;
                var y2 = t2 * t2 + vTwo * t2 + vOne;

                var y = Vector256.ConditionalSelect(mask1, y1, Vector256.ConditionalSelect(mask2, y2, vZero));

                var vBuf = Vector256.LoadUnsafe(ref MemoryMarshal.GetReference(buffer), (nuint)idx);
                (vBuf + y).StoreUnsafe(ref MemoryMarshal.GetReference(buffer), (nuint)idx);

                idx += 8;
            }
        }
        
        // Same as above, but with 128-bit registers.

        else if (Vector128.IsHardwareAccelerated)
        {
            var vDeltaPhase128 = vDeltaPhase.GetLower();
            var vInvDeltaPhase128 = vInvDeltaPhase.GetLower();
            var vOneMinusDelta128 = vOneMinusDelta.GetLower();
            var vOne128 = vOne.GetLower();
            var vTwo128 = vTwo.GetLower();
            var vZero128 = vZero.GetLower();

            while (idx <= buffer.Length - 4)
            {
                var vT = Vector128.LoadUnsafe(ref MemoryMarshal.GetReference(phases), (nuint)idx);

                var mask1 = Vector128.LessThan(vT, vDeltaPhase128);
                var t1 = vT * vInvDeltaPhase128;
                var y1 = vTwo128 * t1 - t1 * t1 - vOne128;

                var mask2 = Vector128.GreaterThan(vT, vOneMinusDelta128);
                var t2 = (vT - vOne128) * vInvDeltaPhase128;
                var y2 = t2 * t2 + vTwo128 * t2 + vOne128;

                var y = Vector128.ConditionalSelect(mask1, y1, Vector128.ConditionalSelect(mask2, y2, vZero128));

                var vBuf = Vector128.LoadUnsafe(ref MemoryMarshal.GetReference(buffer), (nuint)idx);
                (vBuf + y).StoreUnsafe(ref MemoryMarshal.GetReference(buffer), (nuint)idx);

                idx += 4;
            }
        }

        // Scalar version of the interpolation function.
        
        while (idx < buffer.Length)
        {
            var t = phases[idx];
            if (t < deltaPhase)
            {
                t /= deltaPhase;
                buffer[idx] += t + t - t * t - 1f;
            }
            else if (t > 1f - deltaPhase)
            {
                t = (t - 1f) / deltaPhase;
                buffer[idx] += t * t + t + t + 1f;
            }

            idx++;
        }
#else
        // Scalar-only version of the interpolation function.

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
#endif
    }
}