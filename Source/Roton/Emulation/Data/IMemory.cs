using System;

namespace Roton.Emulation.Data
{
    public interface IMemory
    {
        byte[] Dump();
        ReadOnlySpan<byte> Read(int offset, int length);
        int Read8(int offset);
        void Write(int offset, ReadOnlySpan<byte> data);
        void Write8(int offset, int value);
    }
}