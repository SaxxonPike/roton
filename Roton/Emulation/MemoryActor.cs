using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class MemoryActor : Actor
    {
        public MemoryActor(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int Cycle
        {
            get
            {
                return Memory.Read16(Offset + 0x06);
            }
            set
            {
                Memory.Write16(Offset + 0x06, value);
            }
        }

        public override int Follower
        {
            get
            {
                return Memory.Read16(Offset + 0x0B);
            }
            set
            {
                Memory.Write16(Offset + 0x0B, value);
            }
        }

        internal override Heap Heap
        {
            get { return Memory.Heap; }
            set { }
        }

        public override int Instruction
        {
            get
            {
                return Memory.Read16(Offset + 0x15);
            }
            set
            {
                Memory.Write16(Offset + 0x15, value);
            }
        }

        public override bool IsAttached
        {
            get
            {
                return true;
            }
        }

        public override int Leader
        {
            get
            {
                return Memory.Read16(Offset + 0x0D);
            }
            set
            {
                Memory.Write16(Offset + 0x0D, value);
            }
        }

        public override int Length
        {
            get
            {
                return Memory.Read16(Offset + 0x17);
            }
            set
            {
                Memory.Write16(Offset + 0x17, value);
            }
        }

        public override Location Location
        {
            get
            {
                return new MemoryLocation(Memory, Offset + 0x00);
            }
            set
            {
                (new MemoryLocation(Memory, Offset + 0x00)).CopyFrom(value);
            }
        }

        public Memory Memory
        {
            get;
            private set;
        }

        public int Offset
        {
            get;
            private set;
        }

        public override int P1
        {
            get
            {
                return Memory.Read8(Offset + 0x08);
            }
            set
            {
                Memory.Write8(Offset + 0x08, value);
            }
        }

        public override int P2
        {
            get
            {
                return Memory.Read8(Offset + 0x09);
            }
            set
            {
                Memory.Write8(Offset + 0x09, value);
            }
        }

        public override int P3
        {
            get
            {
                return Memory.Read8(Offset + 0x0A);
            }
            set
            {
                Memory.Write8(Offset + 0x0A, value);
            }
        }

        public override int Pointer
        {
            get
            {
                return Memory.Read32(Offset + 0x11);
            }
            set
            {
                Memory.Write32(Offset + 0x11, value);
            }
        }

        public override Tile UnderTile
        {
            get
            {
                return new MemoryTile(Memory, Offset + 0x0F);
            }
            set
            {
                (new MemoryTile(Memory, Offset + 0x0F)).CopyFrom(value);
            }
        }

        public override Vector Vector
        {
            get
            {
                return new MemoryVector(Memory, Offset + 0x02);
            }
            set
            {
                (new MemoryVector(Memory, Offset + 0x02)).CopyFrom(value);
            }
        }
    }
}
