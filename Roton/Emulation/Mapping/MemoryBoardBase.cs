using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryBoardBase : Board
    {
        public MemoryBoardBase(Memory memory)
        {
            Memory = memory;
        }

        public Memory Memory { get; private set; }
    }
}