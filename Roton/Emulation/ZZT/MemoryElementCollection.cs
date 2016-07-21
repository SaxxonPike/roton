using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    internal sealed partial class MemoryElementCollection
    {
        public MemoryElementCollection(IMemory memory, byte[] elementBytes)
            : base(memory)
        {
            memory.Write(0x4AD4, elementBytes);
        }

        protected override IElement GetElement(int index)
        {
            return new MemoryElement(Memory, index);
        }

        public override int Count => 54;
    }
}