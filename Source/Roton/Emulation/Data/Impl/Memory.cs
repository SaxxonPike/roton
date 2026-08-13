using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
[DebuggerStepThrough]
public sealed class Memory : IMemory
{
    private readonly byte[] _data;

    public Memory()
    {
        _data = new byte[0x10000];
    }

    public byte[] Dump()
    {
        var result = new byte[Data.Length];
        Data.CopyTo(result);
        return result;
    }

    public Span<byte> Data
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        get => _data;
    }
}