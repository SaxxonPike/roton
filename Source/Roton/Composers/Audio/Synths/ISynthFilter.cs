namespace Roton.Composers.Audio.Synths;

/// <summary>
/// Contains methods for applying filters to audio signals.
/// </summary>
public interface ISynthFilter
{
    /// <summary>
    /// Applies PolyBLEP interpolation.
    /// </summary>
    float PolyBlep(float halfPhase, float halfPhasePerSample);

    /// <summary>
    /// Applies a one-pole low-pass filter.
    /// </summary>
    float LowPass(float raw, float halfPhasePerSample, float filterState, out float resultFilterState);
}