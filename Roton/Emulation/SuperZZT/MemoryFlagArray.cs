namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryFlagArray : MemoryFlagArrayBase
    {
        public MemoryFlagArray(Memory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count => 16;
    }
}