namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryTile : ITile
    {
        public MemoryTile(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        private IMemory Memory { get; }

        private int Offset { get; }

        public ITile Clone()
        {
            return new Tile(Id, Color);
        }

        public int Color
        {
            get { return Memory.Read8(Offset + 0x01); }
            set { Memory.Write8(Offset + 0x01, value); }
        }

        public int Id
        {
            get { return Memory.Read8(Offset + 0x00); }
            set { Memory.Write8(Offset + 0x00, value); }
        }

        public override string ToString()
        {
            return $"Id: {Id:x2}, Color: {Color:x2}";
        }
    }
}