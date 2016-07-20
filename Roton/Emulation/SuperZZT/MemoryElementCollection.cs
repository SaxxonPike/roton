using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class MemoryElementCollection
    {
        public MemoryElementCollection(IMemory memory)
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