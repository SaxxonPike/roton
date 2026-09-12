using System;
using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class CodeHeap : ICodeHeap
{
    private readonly char[] _block = new char[short.MaxValue - 1];

    public int Size { get; private set; }

    public int Allocate(ReadOnlySpan<char> data)
    {
        if (data.Length == 0)
            return 0;

        var offset = Size;
        Size += data.Length;

        data.CopyTo(_block.AsSpan(offset, data.Length));
        var result = offset | (data.Length << 16);

        return result;
    }

    private Span<char> GetSpan(int pointer)
    {
        var length = unchecked((short)(pointer >> 16));
        var offset = unchecked((short)pointer);

        if (length < 0 || offset < 0)
            return Span<char>.Empty;

        return _block.AsSpan(offset, length);
    }

    public void Free(int pointer)
    {
        // This depends on the actual implementation.
        // Here, we leave the memory intact.
    }

    public void FreeAll()
    {
        _block.AsSpan().Clear();
        Size = 0;
    }

    public IReadOnlyList<(int Index, int Pointer)> Compact(IEnumerable<(int Index, int Pointer)> pointers)
    {
        var newBlock = (stackalloc char[short.MaxValue - 1]);
        var newEntry = 0;

        var result = new List<(int Index, int Pointer)>();

        foreach (var kv in pointers)
        {
            var length = unchecked((short)(kv.Pointer >> 16));
            var offset = unchecked((short)kv.Pointer);
            var data = _block.AsSpan(offset, length);
            var newPointer = (length << 16) + unchecked((short)newEntry);

            data.CopyTo(newBlock.Slice(newEntry));
            result.Add((kv.Index, newPointer));
            newEntry += length;
        }

        newBlock.CopyTo(_block);
        return result;
    }

    public Span<char> this[int pointer] =>
        GetSpan(pointer);
}