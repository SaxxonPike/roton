using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IDrumSoundList : IEnumerable<IDrumSound>
{
    int Count { get; }
    IDrumSound this[int index] { get; }
}