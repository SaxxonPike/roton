using System;
using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal sealed class Memory
    {
        public Memory()
        {
            Bytes = new byte[Length];
            CodeHeap = new CodeHeap();
            Reset();
        }

        private byte[] Bytes { get; }

        public byte[] Dump()
        {
            var result = new byte[Length];
            Array.Copy(Bytes, result, Length);
            return result;
        }

        public CodeHeap CodeHeap { get; }

        public int Length => 0x1 << 16;

        private int Mask => Length - 1;

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

        public int Read16(int offset)
        {
            var result = 0;
            result |= Bytes[(offset + 1) & Mask];
            result <<= 8;
            result |= Bytes[(offset + 0) & Mask];
            result <<= 16;
            result >>= 16;
            return result;
        }

        public int Read32(int offset)
        {
            var result = 0;
            result |= Bytes[(offset + 3) & Mask];
            result <<= 8;
            result |= Bytes[(offset + 2) & Mask];
            result <<= 8;
            result |= Bytes[(offset + 1) & Mask];
            result <<= 8;
            result |= Bytes[(offset + 0) & Mask];
            return result;
        }

        public void Reset()
        {
            for (var i = 0; i < Bytes.Length; i++)
            {
                Bytes[i] = 0x00;
            }
        }

        public bool ReadBool(int offset)
        {
            return Bytes[offset & Mask] != 0;
        }

        public string ReadString(int offset)
        {
            var length = Read8(offset++);
            var result = new byte[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = Bytes[offset++ & Mask];
            }

            return result.ToStringValue();
        }


        public void Write(int offset, byte[] data)
        {
            var length = data.Length;
            for (var i = 0; i < length; i++)
            {
                Bytes[offset++ & Mask] = data[i];
            }
        }

        public void Write(int offset, byte[] data, int dataOffset, int dataLength)
        {
            while (dataLength > 0)
            {
                Bytes[offset++ & Mask] = data[dataOffset++];
                dataLength--;
            }
        }

        public void Write8(int offset, int value)
        {
            Bytes[offset & Mask] = (byte) (value & 0xFF);
        }

        public void Write16(int offset, int value)
        {
            Bytes[offset++ & Mask] = (byte) (value & 0xFF);
            value >>= 8;
            Bytes[offset & Mask] = (byte) (value & 0xFF);
        }

        public void Write32(int offset, int value)
        {
            Bytes[offset++ & Mask] = (byte) (value & 0xFF);
            value >>= 8;
            Bytes[offset++ & Mask] = (byte) (value & 0xFF);
            value >>= 8;
            Bytes[offset++ & Mask] = (byte) (value & 0xFF);
            value >>= 8;
            Bytes[offset & Mask] = (byte) (value & 0xFF);
        }

        public void WriteBool(int offset, bool value)
        {
            Bytes[offset & Mask] = value ? (byte) 1 : (byte) 0;
        }

        public void WriteString(int offset, string value)
        {
            var length = (byte) (value.Length & 0xFF);
            Bytes[offset++ & Mask] = length;
            var encodedString = value.ToBytes();

            for (var i = 0; i < length; i++)
            {
                Bytes[offset++ & Mask] = encodedString[i];
            }
        }
    }
}