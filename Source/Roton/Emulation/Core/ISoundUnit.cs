using System;

namespace Roton.Emulation.Core;

public interface ISoundUnit
{
    void PlaySound(int priority, ReadOnlySpan<byte> sound);
    void ClearSound();
    void PlayStep();
    void PlayErrorSound();
    void UpdateSound();
}