using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class Memory : IMemory
    {
        public Memory()
        {
            Bytes = new byte[Length];
            Heap = new Heap();
            Reset();
        }

        private byte[] Bytes { get; }

        private int Length => 0x1 << 16;

        private int Mask => Length - 1;

        public byte[] Dump()
        {
            var result = new byte[Length];
            Array.Copy(Bytes, result, Length);
            return result;
        }

        public IHeap Heap { get; }

        public byte[] Read(int offset, int length)
        {
            var result = new byte[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = Bytes[offset++ & Mask];
            }
            return result;
        }

        public int Read8(int offset)
        {
            var result = 0;
            result |= Bytes[(offset + 0) & Mask];
            return result;
        }

        public void Write(int offset, byte[] data)
        {
            var length = data.Length;
            for (var i = 0; i < length; i++)
            {
                Bytes[offset++ & Mask] = data[i];
            }
        }

        public void Write8(int offset, int value)
        {
            Bytes[offset & Mask] = (byte) (value & 0xFF);
        }

        private void Reset()
        {
            for (var i = 0; i < Bytes.Length; i++)
            {
                Bytes[i] = 0x00;
            }
        }
    }
}