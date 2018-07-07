using System.Collections.Generic;

namespace Roton.Core
{
    public interface IKeyList : IEnumerable<bool>
    {
        bool this[int index] { get; set; }
        void Clear();
    }
}