namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class MemoryElementCollection : MemoryElementCollectionBase
    {
        public MemoryElementCollection(Memory memory)
            : base(memory)
        {
            memory.Write(0x7CAA, Properties.Resources.szztelement);
        }

        protected override MemoryElementBase GetElement(int index)
        {
            return new MemoryElement(Memory, index);
        }

        public override int Count => 80;
    }
}