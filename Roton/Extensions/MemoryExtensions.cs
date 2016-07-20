using Roton.Core;

namespace Roton.Extensions
{
    public static class MemoryExtensions
    {
        public static int Read16(this IMemory memory, int offset)
        {
            return memory.Read8(offset) | (memory.Read8(offset + 1) << 8);
        }

        public static int Read32(this IMemory memory, int offset)
        {
            return memory.Read8(offset) | 
                (memory.Read8(offset + 1) << 8) |
                (memory.Read8(offset + 2) << 16) |
                (memory.Read8(offset + 3) << 24);
        }

        public static bool ReadBool(this IMemory memory, int offset)
        {
            return memory.Read8(offset) != 0;
        }

        public static string ReadString(this IMemory memory, int offset)
        {
            var length = memory.Read8(offset);
            return memory.Read(offset + 1, length).ToStringValue();
        }

        public static void Write(this IMemory memory, int offset, byte[] data, int dataOffset, int dataLength)
        {
            while (dataLength > 0)
            {
                memory.Write8(offset++, data[dataOffset++]);
                dataLength--;
            }
        }

        public static void Write16(this IMemory memory, int offset, int value)
        {
            memory.Write8(offset, value);
            memory.Write8(offset + 1, value >> 8);
        }

        public static void Write32(this IMemory memory, int offset, int value)
        {
            memory.Write8(offset, value);
            memory.Write8(offset + 1, value >> 8);
            memory.Write8(offset + 2, value >> 16);
            memory.Write8(offset + 3, value >> 24);
        }

        public static void WriteBool(this IMemory memory, int offset, bool value)
        {
            memory.Write8(offset, value ? 1 : 0);
        }

        public static void WriteString(this IMemory memory, int offset, string value)
        {
            var length = value.Length & 0xFF;
            var encodedString = value.ToBytes();
            Write(memory, offset, encodedString, 0, length);
        }
    }
}
