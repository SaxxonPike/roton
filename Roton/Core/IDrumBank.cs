using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Core
{
    public interface IDrumBank : IEnumerable<IDrumSound>
    {
        int Count { get; }
        IDrumSound this[int index] { get; }
    }
}
