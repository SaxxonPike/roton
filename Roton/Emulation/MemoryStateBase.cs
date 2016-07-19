using Roton.Internal;

namespace Roton.Emulation
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