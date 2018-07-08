namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryVector : IXyPair
    {
        public MemoryVector(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        private IMemory Memory { get; }

        private int Offset { get; }

        public IXyPair Clone()
        {
            return new Vector(X, Y);
        }

        public int X
        {
            get { return Memory.Read16(Offset + 0x00); }
            set { Memory.Write16(Offset + 0x00, value); }
        }

        public int Y
        {
            get { return Memory.Read16(Offset + 0x02); }
            set { Memory.Write16(Offset + 0x02, value); }
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}