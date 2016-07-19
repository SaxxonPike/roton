namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryTileCollection : MemoryTileCollectionBase
    {
        public MemoryTileCollection(Memory memory)
            : base(memory, 0x24B9, 60, 25)
        {
        }
    }
}