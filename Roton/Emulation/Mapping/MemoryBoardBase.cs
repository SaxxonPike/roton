using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryBoardBase : Board
    {
        public MemoryBoardBase(IMemory memory)
        {
            Memory = memory;
        }

        public IMemory Memory { get; private set; }
    }
}