using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[DebuggerStepThrough]
public static class MemoryExtensions
{
    [DebuggerStepThrough]
    internal static Span<byte> Read(this IMemory memory, int offset, int length)
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
    internal static int Read8(this IMemory memory, int offset)
    {
        return memory.Data[offset & 0xFFFF];
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Read16(this IMemory memory, int offset)
    {
        unchecked
        {
            var span = memory.Data;
            return ((span[offset & 0xFFFF] | (span[(offset + 1) & 0xFFFF] << 8)) << 16) >> 16;
        }
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Read32(this IMemory memory, int offset)
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
    internal static bool ReadBool(this IMemory memory, int offset)
    {
        return memory.Data[offset & 0xFFFF] != 0;
    }

    [DebuggerStepThrough]
    internal static string ReadString(this IMemory memory, int offset = 0)
    {
        unchecked
        {
            var span = memory.Data;
            var length = span[offset & 0xFFFF];
            var output = new byte[length];
            for (var i = 0; i < length; i++)
                output[i] = span[++offset & 0xFFFF];
            return output.ToStringValue();
        }
    }

    [DebuggerStepThrough]
    internal static void Write(this IMemory memory, int offset, ReadOnlySpan<byte> data)
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
    internal static void Write(this IMemory memory, int offset, ReadOnlySpan<byte> data, int dataOffset, int dataLength)
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
    internal static void Write8(this IMemory memory, int offset, int value)
    {
        unchecked
        {
            var span = memory.Data;
            span[offset & 0xFFFF] = (byte) value;
        }
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Write16(this IMemory memory, int offset, int value)
    {
        unchecked
        {
            var span = memory.Data;
            span[offset & 0xFFFF] = (byte) value;
            span[(offset + 1) & 0xFFFF] = (byte) (value >> 8);
        }
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Write32(this IMemory memory, int offset, int value)
    {
        unchecked
        {
            var span = memory.Data;
            span[offset & 0xFFFF] = (byte) value;
            span[(offset + 1) & 0xFFFF] = (byte) (value >> 8);
            span[(offset + 2) & 0xFFFF] = (byte) (value >> 16);
            span[(offset + 3) & 0xFFFF] = (byte) (value >> 24);
        }
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void WriteBool(this IMemory memory, int offset, bool value)
    {
        unchecked
        {
            var span = memory.Data;
            span[offset & 0xFFFF] = value ? (byte) 1 : (byte) 0;
        }
    }

    [DebuggerStepThrough]
    internal static void WriteString(this IMemory memory, int offset, string value)
    {
        unchecked
        {
            var span = memory.Data;
            var length = value.Length & 0xFF;
            var encodedString = value.ToBytes();
            span[offset++] = (byte) length;
            for (var i = 0; i < length; i++)
                span[offset++] = encodedString[i];
        }
    }
}