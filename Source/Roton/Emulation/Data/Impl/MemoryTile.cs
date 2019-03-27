namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryTile : ITile
    {
        private readonly int _offset;

        internal MemoryTile(IMemory memory, int offset)
        {
            _offset = offset;
            Memory = memory;
        }

        private IMemory Memory { get; }

        public ITile Clone()
        {
            return new Tile(Id, Color);
        }

        public int Color
        {
            get => Memory.Read8(_offset + 0x01);
            set => Memory.Write8(_offset + 0x01, value);
        }

        public int Id
        {
            get => Memory.Read8(_offset + 0x00);
            set => Memory.Write8(_offset + 0x00, value);
        }

        public override string ToString()
        {
            return $"Id: {Id:x2}, Color: {Color:x2}";
        }
    }
}