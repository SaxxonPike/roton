using System;

namespace Roton.Composers.Audio;

public interface ISynthInterpolator
{
    /// <summary>
    /// Applies interpolation.
    /// </summary>
    void Interpolate(Span<float> buffer, ReadOnlySpan<float> phases, float deltaPhase);
}