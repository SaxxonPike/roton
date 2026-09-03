using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Data.Impl;

public abstract class KeyList(IMemory memory, int offset)
    : IKeyList
{
    public int Count => 7;

    public ref Bool this[int index] =>
        ref memory.GetRef<Bool>(offset + index);

    public void Clear() =>
        memory.Data.Slice(offset, Count).Clear();

    public IEnumerator<bool> GetEnumerator() =>
        Enumerable.Range(0, Count)
            .Select(i => (bool)this[i])
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();
}