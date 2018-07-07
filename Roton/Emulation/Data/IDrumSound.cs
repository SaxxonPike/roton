using System.Collections.Generic;

namespace Roton.Core
{
    public interface IDrumSound : IEnumerable<int>
    {
        int Count { get; }
        int this[int index] { get; }
        int Index { get; }
    }
}
