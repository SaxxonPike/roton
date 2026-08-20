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
    private readonly Memory<char>[] _entries = new Memory<char>[256];

    public int Size => _entries.Where(e => !e.IsEmpty).Sum(e => e.Length);

    public int Allocate(ReadOnlySpan<char> data)
    {
        var allocated = new char[data.Length];
        data.CopyTo(allocated);

        while (true)
        {
            if (_nextEntry >= _entries.Length)
                _nextEntry = 1;

            if (_entries[_nextEntry].IsEmpty)
            {
                _entries[_nextEntry] = allocated;
                return _nextEntry++;
            }
        }
    }

    public void Free(int index) =>
        _entries[index] = default;

    public void FreeAll()
    {
        _entries.AsSpan().Clear();
        _nextEntry = 1;
    }

    public Memory<char> this[int index] =>
        _entries[index];

    private bool Contains(int index) =>
        !_entries[index].IsEmpty;
}