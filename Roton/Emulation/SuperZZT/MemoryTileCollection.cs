namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryTileCollection : MemoryTileCollectionBase
    {
        public MemoryTileCollection(Memory memory)
            : base(memory, 0x2BEB, 96, 80)
        {
        }
    }
}