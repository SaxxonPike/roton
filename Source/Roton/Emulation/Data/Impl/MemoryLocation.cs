namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryLocation : IXyPair
    {
        internal MemoryLocation(IMemory memory, int offset)
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
            get => Memory.Read8(Offset + 0x00);
            set => Memory.Write8(Offset + 0x00, value);
        }

        public int Y
        {
            get => Memory.Read8(Offset + 0x01);
            set => Memory.Write8(Offset + 0x01, value);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}