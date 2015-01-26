using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class Memory
    {
        public Memory()
        {
            this.Bytes = new byte[Length];
            this.Heap = new Heap();
            this.Reset();
        }

        private byte[] Bytes
        {
            get;
            set;
        }

        public byte[] Dump()
        {
            byte[] result = new byte[Length];
            Array.Copy(Bytes, result, Length);
            return result;
        }

        public Heap Heap
        {
            get;
            private set;
        }

        public int Length
        {
            get { return 0x1 << 16; }
        }

        private int Mask
        {
            get { return Length - 1; }
        }

        public byte[] Read(int offset, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Bytes[(offset++) & Mask];
            }
            return result;
        }

        public int Read8(int offset)
        {
            Int32 result = 0;
            result |= Bytes[(offset + 0) & Mask];
            return result;
        }

        public int Read16(int offset)
        {
            Int32 result = 0;
            result |= Bytes[(offset + 1) & Mask];
            result <<= 8;
            result |= Bytes[(offset + 0) & Mask];
            result <<= 16;
            result >>= 16;
            return result;
        }

        public int Read32(int offset)
        {
            Int32 result = 0;
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
            for (int i = 0; i < this.Bytes.Length; i++)
            {
                this.Bytes[i] = 0x00;
            }
        }

        public bool ReadBool(int offset)
        {
            return Bytes[offset & Mask] != 0;
        }

        public string ReadString(int offset)
        {
            int length = Read8(offset++);
            byte[] result = new byte[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Bytes[(offset++) & Mask];
            }

            return result.ToStringValue();
        }



        public void Write(int offset, byte[] data)
        {
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                Bytes[(offset++) & Mask] = data[i];
            }
        }

        public void Write(int offset, byte[] data, int dataOffset, int dataLength)
        {
            while (dataLength > 0)
            {
                Bytes[(offset++) & Mask] = data[dataOffset++];
                dataLength--;
            }
        }

        public void Write8(int offset, int value)
        {
            Bytes[(offset) & Mask] = (byte)(value & 0xFF);
        }

        public void Write16(int offset, int value)
        {
            Bytes[(offset++) & Mask] = (byte)(value & 0xFF);
            value >>= 8;
            Bytes[(offset) & Mask] = (byte)(value & 0xFF);
        }

        public void Write32(int offset, int value)
        {
            Bytes[(offset++) & Mask] = (byte)(value & 0xFF);
            value >>= 8;
            Bytes[(offset++) & Mask] = (byte)(value & 0xFF);
            value >>= 8;
            Bytes[(offset++) & Mask] = (byte)(value & 0xFF);
            value >>= 8;
            Bytes[(offset) & Mask] = (byte)(value & 0xFF);
        }

        public void WriteBool(int offset, bool value)
        {
            Bytes[offset & Mask] = value ? (byte)1 : (byte)0;
        }

        public void WriteString(int offset, string value)
        {
            byte length = (byte)(value.Length & 0xFF);
            Bytes[(offset++) & Mask] = length;
            byte[] encodedString = value.ToBytes();

            for (int i = 0; i < length; i++)
            {
                Bytes[(offset++) & Mask] = encodedString[i];
            }
        }
    }
}
