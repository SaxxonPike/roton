using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IDrumSoundList : IEnumerable<IDrumSound>
{
    IDrumSound this[int index] { get; }
}