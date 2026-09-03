using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
[DebuggerStepThrough]
internal sealed class Memory : IMemory
{
    private readonly byte[] _data = new byte[0x10100];

    public byte[] Dump() => 
        Data.ToArray();

    public Span<byte> Data
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        get => _data;
    }
}