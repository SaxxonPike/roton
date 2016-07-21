using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class MemoryElementCollection
    {
        public MemoryElementCollection(IMemory memory, byte[] elementBytes)
            : base(memory)
        {
            memory.Write(0x7CAA, elementBytes);
        }

        protected override IElement GetElement(int index)
        {
            return new MemoryElement(Memory, index);
        }

        public override int Count => 80;
    }
}