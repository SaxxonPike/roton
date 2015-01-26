using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal class MemoryStateBase : State
    {
        public MemoryStateBase(Memory memory)
        {
            this.Memory = memory;
        }

        public Memory Memory
        {
            get;
            private set;
        }

        public virtual int SoundBufferLength { get; set; }
    }
}
