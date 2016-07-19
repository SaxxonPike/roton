using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryStateBase : State
    {
        public MemoryStateBase(Memory memory)
        {
            Memory = memory;
        }

        public Memory Memory { get; private set; }

        public virtual int SoundBufferLength { get; set; }
    }
}