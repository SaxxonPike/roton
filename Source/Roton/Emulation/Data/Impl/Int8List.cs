using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Data.Impl;

internal sealed class Int8List(IMemory memory, int offset, int count) 
    : IRefList<HWord>
{
    public int Count { get; } = count;

    public ref HWord this[int index] =>
        ref memory.GetRef<HWord>(offset + index);

    public IEnumerator<HWord> GetEnumerator() =>
        Enumerable.Range(0, Count)
            .Select(i => this[i])
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();
}