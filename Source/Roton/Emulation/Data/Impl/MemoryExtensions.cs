using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    [DebuggerStepThrough]
    public static class MemoryExtensions
    {
        [DebuggerStepThrough]
        public static int Read16(this IMemory memory, int offset)
        {
            return ((memory.Read8(offset) | (memory.Read8(offset + 1) << 8)) << 16) >> 16;
        }

        [DebuggerStepThrough]
        public static int Read32(this IMemory memory, int offset)
        {
            return memory.Read8(offset) |
                   (memory.Read8(offset + 1) << 8) |
                   (memory.Read8(offset + 2) << 16) |
                   (memory.Read8(offset + 3) << 24);
        }

        [DebuggerStepThrough]
        public static bool ReadBool(this IMemory memory, int offset)
        {
            return memory.Read8(offset) != 0;
        }

        [DebuggerStepThrough]
        public static string ReadString(this IMemory memory, int offset)
        {
            var length = memory.Read8(offset);
            return memory.Read(offset + 1, length).ToArray().ToStringValue();
        }

        [DebuggerStepThrough]
        public static void Write(this IMemory memory, int offset, byte[] data, int dataOffset, int dataLength)
        {
            while (dataLength > 0)
            {
                memory.Write8(offset++, data[dataOffset++]);
                dataLength--;
            }
        }

        [DebuggerStepThrough]
        public static void Write16(this IMemory memory, int offset, int value)
        {
            memory.Write8(offset, value);
            memory.Write8(offset + 1, value >> 8);
        }

        [DebuggerStepThrough]
        public static void Write32(this IMemory memory, int offset, int value)
        {
            memory.Write8(offset, value);
            memory.Write8(offset + 1, value >> 8);
            memory.Write8(offset + 2, value >> 16);
            memory.Write8(offset + 3, value >> 24);
        }

        [DebuggerStepThrough]
        public static void WriteBool(this IMemory memory, int offset, bool value)
        {
            memory.Write8(offset, value ? 1 : 0);
        }

        [DebuggerStepThrough]
        public static void WriteString(this IMemory memory, int offset, string value)
        {
            var length = value.Length & 0xFF;
            var encodedString = value.ToBytes();
            memory.Write8(offset, length);
            Write(memory, offset + 1, encodedString, 0, length);
        }
        
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Read16(this Memory<byte> memory)
        {
            var span = memory.Span;
            return ((span[0] | (span[1] << 8)) << 16) >> 16;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Read16(this Memory<byte> memory, int offset)
        {
            var span = memory.Span.Slice(offset);
            return ((span[0] | (span[1] << 8)) << 16) >> 16;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Read32(this Memory<byte> memory, int offset)
        {
            var span = memory.Span.Slice(offset);
            return span[0] |
                   (span[1] << 8) |
                   (span[2] << 16) |
                   (span[3] << 24);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(this Memory<byte> memory, int offset)
        {
            return memory.Span[offset] != 0;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(this Memory<byte> memory, int offset)
        {
            var span = memory.Span;
            var length = span[offset];
            return span.Slice(offset + 1, length).ToArray().ToStringValue();
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Memory<byte> memory, int offset, byte[] data, int dataOffset, int dataLength)
        {
            data.AsSpan(dataOffset, dataLength).CopyTo(memory.Span.Slice(offset));
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write8(this Memory<byte> memory, int offset, int value)
        {
            var span = memory.Span;
            span[offset] = unchecked((byte) value);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write16(this Memory<byte> memory, int value)
        {
            var span = memory.Span;
            span[0] = unchecked((byte) value);
            span[1] = unchecked((byte) (value >> 8));
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write16(this Memory<byte> memory, int offset, int value)
        {
            var span = memory.Span.Slice(offset);
            span[0] = unchecked((byte) value);
            span[1] = unchecked((byte) (value >> 8));
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write32(this Memory<byte> memory, int offset, int value)
        {
            var span = memory.Span.Slice(offset);
            span[0] = unchecked((byte) value);
            span[1] = unchecked((byte) (value >> 8));
            span[2] = unchecked((byte) (value >> 16));
            span[3] = unchecked((byte) (value >> 24));
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBool(this Memory<byte> memory, int offset, bool value)
        {
            var span = memory.Span;
            span[offset] = value ? (byte) 1 : (byte) 0;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Memory<byte> memory, int offset, string value)
        {
            var length = value.Length & 0xFF;
            var encodedString = value.ToBytes();
            var span = memory.Span.Slice(offset);
            span[0] = unchecked((byte) length);
            encodedString.AsSpan().CopyTo(span.Slice(1));
        }
    }
}