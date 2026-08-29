using System;

namespace Roton.Composers.Audio.Drums;

/// <summary>
/// Renders drum sounds to an active audio buffer.
/// </summary>
public interface IDrumComposer
{
    /// <summary>
    /// Starts playing a drum sound.
    /// </summary>
    /// <param name="frequencies">
    /// Frequency sequence to play, where values are in hz.
    /// </param>
    /// <param name="rate">
    /// Rate at which to advance through the frequency sequence in hz.
    /// </param>
    void SetDrum(ReadOnlySpan<int> frequencies, float rate);

    /// <summary>
    /// Stops playing drum sounds.
    /// </summary>
    void ClearDrum();

    /// <summary>
    /// Renders drum sounds to an active audio buffer.
    /// </summary>
    /// <param name="buffer">
    /// Buffer that will receive drum sound samples.
    /// </param>
    /// <returns>
    /// Number of samples rendered.
    /// </returns>
    /// <remarks>
    /// If no drum sound is playing, no samples will be written to the buffer.
    /// </remarks>
    int ComposeDrum(Span<float> buffer);
}