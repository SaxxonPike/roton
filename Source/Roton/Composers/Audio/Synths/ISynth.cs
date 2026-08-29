using System;

namespace Roton.Composers.Audio.Synths;

/// <summary>
/// Renders synthesizer tones to an active audio buffer.
/// </summary>
public interface ISynth
{
    /// <summary>
    /// Sets the wave synthesizer output frequency.
    /// </summary>
    /// <param name="frequency">
    /// Frequency in hz.
    /// </param>
    void SetFrequency(float frequency);

    /// <summary>
    /// Updates the synthesizer counters internally. If the sample
    /// rate changes, this should be invoked.
    /// </summary>
    void Update();

    /// <summary>
    /// Renders the synthesizer output to the provided buffer.
    /// </summary>
    /// <param name="buffer">
    /// Buffer that will receive the synthesizer output.
    /// </param>
    /// <returns>
    /// The number of samples rendered.
    /// </returns>
    int Render(Span<float> buffer);
}