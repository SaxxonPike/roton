using System;

namespace Roton.Composers.Audio.Steps;

/// <summary>
/// Renders footstep sounds to an active audio buffer.
/// </summary>
public interface IStepComposer
{
    /// <summary>
    /// Starts playing a footstep sound.
    /// </summary>
    void SetStep();
    
    /// <summary>
    /// Stops playing footstep sounds.
    /// </summary>
    void ClearStep();
    
    /// <summary>
    /// Renders footstep sounds to an audio buffer.
    /// </summary>
    /// <param name="buffer">
    /// Buffer that will receive footstep sound samples.
    /// </param>
    /// <returns>
    /// Number of samples written to the buffer.
    /// </returns>
    /// <remarks>
    /// If no footstep sound is playing, no samples will be written to the buffer.
    /// </remarks>
    int ComposeStep(Span<float> buffer);
}