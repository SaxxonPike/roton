using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IKeyList : IEnumerable<bool>
{
    ref Bool this[int index] { get; }
    void Clear();
}