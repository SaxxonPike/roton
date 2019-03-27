using System;
using System.Diagnostics;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    [DebuggerStepThrough]
    public sealed class Memory : IMemory
    {
        private readonly byte[] _bytes;

        public Memory()
        {
            _bytes = new byte[Length];
            Reset();
        }

        private const int Length = 0x1 << 16;

        private const int Mask = Length - 1;

        public byte[] Dump()
        {
            var result = new byte[Length];
            Buffer.BlockCopy(_bytes, 0, result, 0, Length);
            return result;
        }

        [DebuggerStepThrough]
        public ReadOnlySpan<byte> Read(int offset, int length)
        {
            if (offset + length <= Length) 
                return _bytes.AsSpan(offset, length);

            var result = new byte[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = _bytes[offset++ & Mask];
            }
            return result;
        }

        [DebuggerStepThrough]
        public int Read8(int offset)
        {
            var result = 0;
            result |= _bytes[offset & Mask];
            return result;
        }

        [DebuggerStepThrough]
        public void Write(int offset, ReadOnlySpan<byte> data)
        {
            if (offset + data.Length <= Length)
            {
                data.CopyTo(_bytes.AsSpan(offset));
                return;
            }

            var length = data.Length;
            for (var i = 0; i < length; i++)
            {
                _bytes[offset++ & Mask] = data[i];
            }
        }

        [DebuggerStepThrough]
        public void Write8(int offset, int value)
        {
            _bytes[offset & Mask] = (byte) (value & 0xFF);
        }

        [DebuggerStepThrough]
        private void Reset()
        {
            for (var i = 0; i < _bytes.Length; i++)
            {
                _bytes[i] = 0x00;
            }
        }
    }
}