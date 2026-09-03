using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data;

public static class MemoryExtensions
{
    extension(IMemory memory)
    {
        internal ref T GetRef<T>(int offset) where T : struct =>
            ref Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(MemoryMarshal.Cast<byte, T>(memory.Data)), unchecked((ushort)offset));
            // ref MemoryMarshal.Cast<byte, T>(memory.Data.Slice(unchecked((ushort)offset)))[0];

        internal Span<byte> Read(int offset, int length)
        {
            unchecked
            {
                var span = memory.Data;
                var output = new byte[length];
                for (var i = 0; i < length; i++)
                    output[i] = span[offset++ & 0xFFFF];
                return output;
            }
        }

        internal int Read8(int offset)
        {
            return memory.Data[offset & 0xFFFF];
        }

        internal string ReadString(int offset = 0)
        {
            unchecked
            {
                var span = memory.Data;
                var length = span[offset & 0xFFFF];
                var end = offset + length;
                var output = new byte[length];

                if (end <= span.Length)
                    return span.Slice(offset + 1, length).ToStringValue();
                
                for (var i = 0; i < length; i++)
                    output[i] = span[++offset & 0xFFFF];
                return output.ToStringValue();
            }
        }

        internal ReadOnlySpan<byte> ReadStringSpan(int offset = 0)
        {
            unchecked
            {
                var span = memory.Data;
                var length = span[offset & 0xFFFF];

                if (offset + length <= memory.Data.Length)
                    return span.Slice(offset + 1, length);

                var result = new byte[length];
                for (var i = 0; i < length; i++)
                    result[i] = span[++offset & 0xFFFF];
                return result;
            }
        }

        internal ReadOnlySpan<byte> ReadStringSpan(int offset, Span<byte> buffer)
        {
            unchecked
            {
                var span = memory.Data;
                var length = span[offset & 0xFFFF];

                if (offset + length <= memory.Data.Length)
                    return span.Slice(offset + 1, length);

                var result = buffer.Slice(0, length);
                for (var i = 0; i < length; i++)
                    result[i] = span[++offset & 0xFFFF];
                return result;
            }
        }

        internal void Write(int offset, ReadOnlySpan<byte> data)
        {
            unchecked
            {
                var span = memory.Data;
                var dataLength = data.Length;
                for (var i = 0; i < dataLength; i++)
                    span[offset++ & 0xFFFF] = data[i];
            }
        }

        internal void Write(int offset, ReadOnlySpan<byte> data, int dataOffset, int dataLength)
        {
            unchecked
            {
                var span = memory.Data;
                for (var i = 0; i < dataLength; i++)
                    span[offset++ & 0xFFFF] = data[dataOffset++];
            }
        }

        internal void Write8(int offset, int value)
        {
            unchecked
            {
                var span = memory.Data;
                span[offset & 0xFFFF] = (byte)value;
            }
        }

        internal void Write16(int offset, int value)
        {
            unchecked
            {
                var span = memory.Data;
                span[offset & 0xFFFF] = (byte)value;
                span[(offset + 1) & 0xFFFF] = (byte)(value >> 8);
            }
        }

        internal void WriteString(int offset, ReadOnlySpan<char> value)
        {
            unchecked
            {
                var span = memory.Data;
                var length = value.Length & 0xFF;
                span[offset & 0xFFFF] = (byte)length;
                if (length > 0)
                {
                    var destination = span.Slice((offset + 1) & 0xFFFF);
                    // Handle wrap-around if necessary
                    if (destination.Length >= length)
                    {
                        value.ToBytes(destination.Slice(0, length));
                    }
                    else
                    {
                        // Fallback for wrap-around
                        for (var i = 0; i < length; i++)
                            span[(offset + 1 + i) & 0xFFFF] = Cp437.CharToByte(value[i]);
                    }
                }
            }
        }
    }
}