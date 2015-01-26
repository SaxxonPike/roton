using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal sealed class MemoryTile : Tile
    {
        public MemoryTile(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int Color
        {
            get
            {
                return Memory.Read8(Offset + 0x01);
            }
            set
            {
                Memory.Write8(Offset + 0x01, value);
            }
        }

        public override int Id
        {
            get
            {
                return Memory.Read8(Offset + 0x00);
            }
            set
            {
                Memory.Write8(Offset + 0x00, value);
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
    }
}
