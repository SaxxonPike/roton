using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Data.Impl;

internal sealed class ByteString(IMemory memory, int offset)
    : IRefList<PChar>
{
    public int Count =>
        memory.Read8(offset);

    public ref PChar this[int index] =>
        ref memory.GetRef<PChar>(offset + index + 1);

    public IEnumerator<PChar> GetEnumerator() =>
        Enumerable.Range(1, Count)
            .Select(i => this[i])
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}