using Roton.Emulation.Core;

namespace Lyon.Presenters;

/// <summary>
/// Handles audio playback.
/// </summary>
public interface IAudioPresenter
{
    /// <summary>
    /// Open the audio device and start streaming audio.
    /// </summary>
    void Start(IEngine engine);

    /// <summary>
    /// Stops streaming audio and closes the audio device.
    /// </summary>
    void Stop();
}