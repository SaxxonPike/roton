using System.Collections.Generic;

namespace Roton.Emulation.Data
{
    public interface IKeyList : IEnumerable<bool>
    {
        bool this[int index] { get; set; }
        void Clear();
    }
}