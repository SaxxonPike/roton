using Roton.Internal;

namespace Roton.Emulation
{
    internal sealed class MemoryActor : Actor
    {
        public MemoryActor(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override char[] Code
        {
            get { return Memory.CodeHeap[Pointer]; }
            set { }
        }

        public override int Cycle
        {
            get { return Memory.Read16(Offset + 0x06); }
            set { Memory.Write16(Offset + 0x06, value); }
        }

        public override int Follower
        {
            get { return Memory.Read16(Offset + 0x0B); }
            set { Memory.Write16(Offset + 0x0B, value); }
        }

        public override int Instruction
        {
            get { return Memory.Read16(Offset + 0x15); }
            set { Memory.Write16(Offset + 0x15, value); }
        }

        public override bool IsAttached => true;

        public override int Leader
        {
            get { return Memory.Read16(Offset + 0x0D); }
            set { Memory.Write16(Offset + 0x0D, value); }
        }

        public override int Length
        {
            get { return Memory.Read16(Offset + 0x17); }
            set { Memory.Write16(Offset + 0x17, value); }
        }

        public override IXyPair Location => new MemoryLocation(Memory, Offset + 0x00);

        public Memory Memory { get; }

        public int Offset { get; }

        public override int P1
        {
            get { return Memory.Read8(Offset + 0x08); }
            set { Memory.Write8(Offset + 0x08, value); }
        }

        public override int P2
        {
            get { return Memory.Read8(Offset + 0x09); }
            set { Memory.Write8(Offset + 0x09, value); }
        }

        public override int P3
        {
            get { return Memory.Read8(Offset + 0x0A); }
            set { Memory.Write8(Offset + 0x0A, value); }
        }

        public override int Pointer
        {
            get { return Memory.Read32(Offset + 0x11); }
            set { Memory.Write32(Offset + 0x11, value); }
        }

        public override ITile UnderTile => new MemoryTile(Memory, Offset + 0x0F);

        public override IXyPair Vector => new MemoryVector(Memory, Offset + 0x02);
    }
}