using System;

namespace Roton.Emulation.Core;

public interface ISoundPlayer
{
    /// <remarks>
    /// RoZ: SoundQueue
    /// </remarks>
    void PlaySound(int priority, ReadOnlySpan<byte> sound);

    /// <remarks>
    /// RoZ: SoundClearQueue
    /// </remarks>
    void ClearSound();

    void PlayStep();

    /// <remarks>
    /// RoZ: PauseOnError (without delay)
    /// </remarks>
    void PlayErrorSound();

    /// <remarks>
    /// RoZ: SoundTimerHandler
    /// </remarks>
    void UpdateSound();
}