using System;

namespace Roton.Emulation.Data
{
    public interface IMemory
    {
        byte[] Dump();
        Span<byte> Data { get; }
    }
}