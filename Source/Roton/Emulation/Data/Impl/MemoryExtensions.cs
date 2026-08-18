using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[DebuggerStepThrough]
public static class MemoryExtensions
{
    extension(IMemory memory)
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetRef<T>(int offset) where T : struct => 
            ref MemoryMarshal.Cast<byte, T>(memory.Data.Slice(offset))[0];

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int Read8(int offset)
        {
            return memory.Data[offset & 0xFFFF];
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int FastRead16(int offset) =>
            BinaryPrimitives.ReadInt16LittleEndian(memory.Data.Slice(offset));

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int Read16(int offset)
        {
            var span = memory.Data;
            if (offset < 0xFFFF)
                return BinaryPrimitives.ReadInt16LittleEndian(span.Slice(offset));
            return unchecked((short)(span[offset & 0xFFFF] | (span[(offset + 1) & 0xFFFF] << 8)));
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int Read32(int offset)
        {
            unchecked
            {
                var span = memory.Data;
                return span[offset & 0xFFFF] |
                       (span[(offset + 1) & 0xFFFF] << 8) |
                       (span[(offset + 2) & 0xFFFF] << 16) |
                       (span[(offset + 3) & 0xFFFF] << 24);
            }
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool ReadBool(int offset)
        {
            return memory.Data[offset & 0xFFFF] != 0;
        }

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
        internal void Write(int offset, ReadOnlySpan<byte> data, int dataOffset, int dataLength)
        {
            unchecked
            {
                var span = memory.Data;
                for (var i = 0; i < dataLength; i++)
                    span[offset++ & 0xFFFF] = data[dataOffset++];
            }
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write8(int offset, int value)
        {
            unchecked
            {
                var span = memory.Data;
                span[offset & 0xFFFF] = (byte)value;
            }
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void FastWrite16(int offset, int value) =>
            BinaryPrimitives.WriteInt16LittleEndian(memory.Data.Slice(offset), unchecked((short)value));


        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write16(int offset, int value)
        {
            unchecked
            {
                var span = memory.Data;
                span[offset & 0xFFFF] = (byte)value;
                span[(offset + 1) & 0xFFFF] = (byte)(value >> 8);
            }
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write32(int offset, int value)
        {
            unchecked
            {
                var span = memory.Data;
                span[offset & 0xFFFF] = (byte)value;
                span[(offset + 1) & 0xFFFF] = (byte)(value >> 8);
                span[(offset + 2) & 0xFFFF] = (byte)(value >> 16);
                span[(offset + 3) & 0xFFFF] = (byte)(value >> 24);
            }
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void WriteBool(int offset, bool value)
        {
            unchecked
            {
                var span = memory.Data;
                span[offset & 0xFFFF] = value ? (byte)1 : (byte)0;
            }
        }

        [DebuggerStepThrough]
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
                            span[(offset + 1 + i) & 0xFFFF] = Cp437.CharToByte(value![i]);
                    }
                }
            }
        }
    }
}