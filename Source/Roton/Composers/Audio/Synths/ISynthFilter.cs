namespace Roton.Composers.Audio.Synths;

/// <summary>
/// Contains methods for applying filters to audio signals.
/// </summary>
public interface ISynthFilter
{
    /// <summary>
    /// Updates coefficients in the filter.
    /// </summary>
    /// <param name="cutoff">
    /// Cutoff frequency in Hz.
    /// </param>
    /// <param name="sampleRate">
    /// Sample rate of the generated data in Hz.
    /// </param>
    void Update(float cutoff, float sampleRate);

    /// <summary>
    /// Applies the filter.
    /// </summary>
    /// <param name="x">
    /// The input sample.
    /// </param>
    /// <returns>
    /// The processed sample.
    /// </returns>
    float LowPass(float x);
}