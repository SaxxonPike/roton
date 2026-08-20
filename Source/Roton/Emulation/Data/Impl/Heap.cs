using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Heap : IHeap
{
    private int _nextEntry = 1;

    private IDictionary<int, Memory<char>> Entries { get; } = new Dictionary<int, Memory<char>>();

    public int Size => Entries.Where(e => !e.Value.IsEmpty).Sum(e => e.Value.Length);

    public int Allocate(ReadOnlySpan<char> data)
    {
        var allocated = new char[data.Length];
        data.CopyTo(allocated);

        int index;

        while (Entries.ContainsKey(index = Interlocked.Increment(ref _nextEntry)))
        {
        }

        Entries[_nextEntry] = allocated;
        return index;
    }

    public void FreeAll()
    {
        Entries.Clear();
        _nextEntry = 1;
    }

    public Memory<char> this[int index] =>
        Entries.ContainsKey(index)
            ? Entries[index]
            : null;

    private bool Contains(int index) => Entries.ContainsKey(index);
}