using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal class MemoryElementBase : Element
    {
        public MemoryElementBase(Memory memory, int offset)
        {
            this.Memory = memory;
            this.Offset = offset;
            this.Act = DefaultAct;
            this.Draw = DefaultDraw;
            this.Interact = DefaultInteract;
        }

        abstract internal void CopyFrom(MemoryElementBase other);

        static public void DefaultAct(int index)
        {
        }

        static public AnsiChar DefaultDraw(Location location)
        {
            return new AnsiChar(0x3F, 0x40);
        }

        static public void DefaultInteract(Location location, int index, Vector vector)
        {
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
