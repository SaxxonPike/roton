using System;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Actor : IActor
    {
        private readonly IMemory _memory;
        private readonly IHeap _heap;

        public Actor(IMemory memory, IHeap heap, int offset)
        {
            _memory = memory;
            _heap = heap;
            Offset = offset;
            Location = new MemoryLocation(_memory, Offset + 0x00);
            UnderTile = new MemoryTile(_memory, Offset + 0x0F);
            Vector = new MemoryVector(_memory, Offset + 0x02);
        }

        public int Offset { get; }

        public int Cycle
        {
            get => _memory.Read16(Offset + 0x06);
            set => _memory.Write16(Offset + 0x06, value);
        }

        public int Follower
        {
            get => _memory.Read16(Offset + 0x0B);
            set => _memory.Write16(Offset + 0x0B, value);
        }

        public int Leader
        {
            get => _memory.Read16(Offset + 0x0D);
            set => _memory.Write16(Offset + 0x0D, value);
        }

        public int Length
        {
            get => _memory.Read16(Offset + 0x17);
            set => _memory.Write16(Offset + 0x17, value);
        }

        public IXyPair Location { get; }

        public int P1
        {
            get => _memory.Read8(Offset + 0x08);
            set => _memory.Write8(Offset + 0x08, value);
        }

        public int P2
        {
            get => _memory.Read8(Offset + 0x09);
            set => _memory.Write8(Offset + 0x09, value);
        }

        public int P3
        {
            get => _memory.Read8(Offset + 0x0A);
            set => _memory.Write8(Offset + 0x0A, value);
        }

        public int Pointer
        {
            get => _memory.Read32(Offset + 0x11);
            set => _memory.Write32(Offset + 0x11, value);
        }

        public ITile UnderTile { get; }

        public IXyPair Vector { get; }

        public int Instruction
        {
            get => _memory.Read16(Offset + 0x15);
            set => _memory.Write16(Offset + 0x15, value);
        }

        public byte[] Code
        {
            get => _heap[Pointer];
            set { }
        }

        public override string ToString()
        {
            var name = string.Empty;
            if (Code != null)
            {
                // walk the code to get the name
                var data = Code;
                if (data[0] == 0x40)
                {
                    var length = data.Length;
                    for (var i = 1; i < length; i++)
                    {
                        if (data[i] == 0x0D)
                        {
                            var nameData = new byte[i - 1];
                            Array.Copy(data, 1, nameData, 0, nameData.Length);
                            name = nameData.ToStringValue();
                            break;
                        }
                    }
                }
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : $" {name}";
            }
            name = Location + name;
            return name;
        }
    }
}