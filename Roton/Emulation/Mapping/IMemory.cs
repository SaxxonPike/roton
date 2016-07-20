using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.Mapping
{
    public interface IMemory
    {
        CodeHeap CodeHeap { get; }
        byte[] Dump();
        int Length { get; }
        byte[] Read(int offset, int length);
        int Read8(int offset);
        void Reset();
        void Write(int offset, byte[] data);
        void Write8(int offset, int value);
    }
}
