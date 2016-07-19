namespace Roton.Emulation.ZZT
{
    internal class MemoryActorCollection : MemoryActorCollectionBase
    {
        public MemoryActorCollection(Memory memory)
            : base(memory)
        {
        }

        public override int Capacity => 152;

        public override int Count => Memory.Read16(0x31CD) + 1;

        protected override MemoryActor GetActor(int index)
        {
            return new MemoryActor(Memory, 0x31CF + 0x0021*index);
        }
    }
}