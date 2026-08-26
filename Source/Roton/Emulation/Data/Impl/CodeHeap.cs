using System;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CodeHeap(ITracer tracer)
    : ICodeHeap
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

    public void Free(int index)
    {
        if (index < 0 || index >= _entries.Length)
        {
            // Scrolls without an associated actor cause this crash when touched.

            tracer.TraceCrash("Attempted to free invalid index");
            return;
        }

        _entries[index] = default;
    }

    public void FreeAll()
    {
        _entries.AsSpan().Clear();
        _nextEntry = 1;
    }

    public Memory<char> this[int index] =>
        index >= 0 && index < _entries.Length
            ? _entries[index]
            : Memory<char>.Empty;

    private bool Contains(int index) =>
        !_entries[index].IsEmpty;
}