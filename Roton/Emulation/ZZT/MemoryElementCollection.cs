namespace Roton.Emulation.ZZT
{
    internal sealed partial class MemoryElementCollection
    {
        public MemoryElementCollection(Memory memory)
            : base(memory)
        {
            memory.Write(0x4AD4, Properties.Resources.zztelement);
        }

        protected override MemoryElementBase GetElement(int index)
        {
            return new MemoryElement(Memory, index);
        }

        public override int Count => 54;
    }
}