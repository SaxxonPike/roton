using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IDrumSound : IEnumerable<int>
{
    int Count { get; }
    int this[int index] { get; }
    int Index { get; }
}