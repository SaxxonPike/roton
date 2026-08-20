using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CodeHeap : ICodeHeap
{
    private int _nextEntry = 1;
    private readonly SortedDictionary<int, Memory<char>> _entries = [];

    public int Size => _entries.Where(e => !e.Value.IsEmpty).Sum(e => e.Value.Length);

    public int Allocate(ReadOnlySpan<char> data)
    {
        var allocated = new char[data.Length];
        data.CopyTo(allocated);

        int index;

        while (_entries.ContainsKey(index = Interlocked.Increment(ref _nextEntry)))
        {
        }

        _entries[_nextEntry] = allocated;
        return index;
    }

    public void Free(int index) =>
        _entries.Remove(index);

    public void FreeAll()
    {
        _entries.Clear();
        _nextEntry = 1;
    }

    public Memory<char> this[int index] =>
        _entries.ContainsKey(index)
            ? _entries[index]
            : null;

    private bool Contains(int index) =>
        _entries.ContainsKey(index);
}