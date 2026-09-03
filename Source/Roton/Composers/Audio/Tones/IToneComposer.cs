using System;

namespace Roton.Composers.Audio.Tones;

/// <summary>
/// Renders music tones to an active audio buffer.
/// </summary>
public interface IToneComposer
{
    /// <summary>
    /// Sets the tone to play.
    /// </summary>
    /// <param name="toneNumber">
    /// Tone that should be played.
    /// </param>
    /// <remarks>
    /// Tone number is in semitones, and 45 = A440.
    /// </remarks>
    void SetTone(int toneNumber);

    /// <summary>
    /// Stops playing tones.
    /// </summary>
    void ClearTone();

    /// <summary>
    /// Renders the tone waveform to the buffer.
    /// </summary>
    /// <param name="buffer">
    /// Buffer that will receive the tone waveform.
    /// </param>
    /// <returns>
    /// Number of samples written to the buffer.
    /// </returns>
    /// <remarks>
    /// If no music tone is playing, no samples will be written to the buffer.
    /// </remarks>
    int ComposeTone(Span<float> buffer);
}