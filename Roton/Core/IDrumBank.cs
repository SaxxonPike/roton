using System.Collections.Generic;

namespace Roton.Core
{
    public interface IDrumBank : IEnumerable<IDrumSound>
    {
        int Count { get; }
        IDrumSound this[int index] { get; }
    }
}
