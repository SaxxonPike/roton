using Roton.Core;
using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryBoardBase : Board
    {
        protected MemoryBoardBase(IMemory memory)
        {
            Memory = memory;
        }

        public IMemory Memory { get; private set; }
    }
}