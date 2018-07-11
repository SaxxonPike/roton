using System;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class Actor : IActor
    {
        public Actor(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
            Location = new MemoryLocation(Memory, Offset + 0x00);
            UnderTile = new MemoryTile(Memory, Offset + 0x0F);
            Vector = new MemoryVector(Memory, Offset + 0x02);
        }

        private IMemory Memory { get; }

        public int Offset { get; }

        public int Cycle
        {
            get => Memory.Read16(Offset + 0x06);
            set => Memory.Write16(Offset + 0x06, value);
        }

        public int Follower
        {
            get => Memory.Read16(Offset + 0x0B);
            set => Memory.Write16(Offset + 0x0B, value);
        }

        public int Leader
        {
            get => Memory.Read16(Offset + 0x0D);
            set => Memory.Write16(Offset + 0x0D, value);
        }

        public int Length
        {
            get => Memory.Read16(Offset + 0x17);
            set => Memory.Write16(Offset + 0x17, value);
        }

        public IXyPair Location { get; }

        public int P1
        {
            get => Memory.Read8(Offset + 0x08);
            set => Memory.Write8(Offset + 0x08, value);
        }

        public int P2
        {
            get => Memory.Read8(Offset + 0x09);
            set => Memory.Write8(Offset + 0x09, value);
        }

        public int P3
        {
            get => Memory.Read8(Offset + 0x0A);
            set => Memory.Write8(Offset + 0x0A, value);
        }

        public int Pointer
        {
            get => Memory.Read32(Offset + 0x11);
            set => Memory.Write32(Offset + 0x11, value);
        }

        public ITile UnderTile { get; }

        public IXyPair Vector { get; }

        public int Instruction
        {
            get => Memory.Read16(Offset + 0x15);
            set => Memory.Write16(Offset + 0x15, value);
        }

        public byte[] Code
        {
            get => Memory.Heap[Pointer];
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