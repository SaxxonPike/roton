using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal abstract partial class MemoryTileCollectionBase : FixedList<Tile>
    {
        public MemoryTileCollectionBase(Memory memory, int offset, int width, int height)
        {
            this.Memory = memory;
            this.Offset = offset;
            this.Height = height;
            this.Width = width;
        }

        public override Tile this[int index]
        {
            get
            {
                return new MemoryTile(Memory, Offset + (index * 2));
            }
            set
            {
                this[index].CopyFrom(value);
            }
        }

        public Tile this[Location location]
        {
            get
            {
                return this[(location.X * TotalHeight) + location.Y];
            }
            set
            {
                this[location].CopyFrom(value);
            }
        }

        public override int Count
        {
            get { return TotalWidth * TotalHeight; }
        }

        public int Height
        {
            get;
            private set;
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

        private int TotalHeight
        {
            get
            {
                return Height + 2;
            }
        }

        private int TotalWidth
        {
            get
            {
                return Width + 2;
            }
        }

        public int Width
        {
            get;
            private set;
        }
    }
}
