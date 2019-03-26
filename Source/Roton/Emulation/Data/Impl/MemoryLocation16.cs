namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryLocation16 : IXyPair
    {
        internal MemoryLocation16(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        private IMemory Memory { get; }

        private int Offset { get; }

        public IXyPair Clone()
        {
            return new Location(X, Y);
        }

        public int X
        {
            get => Memory.Read16(Offset + 0x00);
            set => Memory.Write16(Offset + 0x00, value);
        }

        public int Y
        {
            get => Memory.Read16(Offset + 0x02);
            set => Memory.Write16(Offset + 0x02, value);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}