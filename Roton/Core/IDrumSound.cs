using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Core
{
    public interface IDrumSound : IEnumerable<int>
    {
        int Count { get; }
        int this[int index] { get; }
        int Index { get; }
    }
}
