using System;

namespace Roton.Composers.Audio.Synths;

public interface ISynthInterpolator
{
    /// <summary>
    /// Applies interpolation.
    /// </summary>
    void Interpolate(Span<float> buffer, ReadOnlySpan<float> phases, float deltaPhase);
}