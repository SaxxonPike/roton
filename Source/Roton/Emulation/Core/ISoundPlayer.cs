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

    /// <summary>
    /// RoZ: PauseOnError (without delay)
    /// </summary>
    void PlayErrorSound();

    void UpdateSound();
}