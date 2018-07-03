using Roton.Core;

namespace Roton.Emulation.Mapping
{
    public sealed class MemoryLocation : IXyPair
    {
        public MemoryLocation(IMemory memory, int offset)
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
            get { return Memory.Read8(Offset + 0x00); }
            set { Memory.Write8(Offset + 0x00, value); }
        }

        public int Y
        {
            get { return Memory.Read8(Offset + 0x01); }
            set { Memory.Write8(Offset + 0x01, value); }
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}