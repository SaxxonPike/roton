using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryStateBase : State
    {
        public MemoryStateBase(IMemory memory)
        {
            Memory = memory;
        }

        public IMemory Memory { get; private set; }

        public virtual int SoundBufferLength { get; set; }
    }
}