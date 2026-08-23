using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ISoundUnit
{
    void PlaySound(int priority, ISound sound, int? offset = null, int? length = null);
    void ClearSound();
    void PlayStep();
    void PlayErrorSound();
}