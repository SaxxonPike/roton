using System.Collections.Generic;

namespace Roton.Emulation.Data
{
    public interface IDrumBank : IEnumerable<IDrumSound>
    {
        int Count { get; }
        IDrumSound this[int index] { get; }
    }
}
