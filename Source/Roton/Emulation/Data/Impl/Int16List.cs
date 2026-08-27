using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Data.Impl;

public sealed class Int16List(IMemory memory, int offset, int count) 
    : IRefList<Word>
{
    public int Count { get; } = count;

    public ref Word this[int index] =>
        ref memory.GetRef<Word>(offset + (index << 1));

    public IEnumerator<Word> GetEnumerator() =>
        Enumerable.Range(0, Count)
            .Select(i => this[i])
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();
}