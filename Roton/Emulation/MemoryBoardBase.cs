namespace Roton.Emulation
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