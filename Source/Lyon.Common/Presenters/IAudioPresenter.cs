namespace Lyon.Common.Presenters;

/// <summary>
/// Handles audio playback.
/// </summary>
public interface IAudioPresenter
{
    /// <summary>
    /// Open the audio device and start streaming audio.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops streaming audio and closes the audio device.
    /// </summary>
    void Stop();

    /// <summary>
    /// Output gain of the audio signal. 1.0 is full volume, 0.0 is silent.
    /// </summary>
    float Gain { get; set; }
}