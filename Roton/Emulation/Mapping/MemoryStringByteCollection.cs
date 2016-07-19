﻿using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal sealed class MemoryStringByteCollection : FixedList<int>
    {
        public MemoryStringByteCollection(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override int GetItem(int index)
        {
            return Memory.Read8(Offset + index + 1);
        }

        protected override void SetItem(int index, int value)
        {
            Memory.Write8(Offset + index + 1, value);
        }

        public override int Count => Memory.Read8(Offset);

        public Memory Memory { get; }

        public int Offset { get; }
    }
}