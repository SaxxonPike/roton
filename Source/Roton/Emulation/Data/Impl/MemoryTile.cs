namespace Roton.Emulation.Data.Impl;

public sealed class MemoryTile : ITile
{
    private readonly int _offset;
    private readonly IMemory _memory;

    internal MemoryTile(IMemory memory, int offset)
    {
        _offset = offset;
        _memory = memory;
    }

    public ITile Clone()
    {
        return new Tile(Id, Color);
    }

    public int Color
    {
        get => _memory.Read8(_offset + 0x01);
        set => _memory.Write8(_offset + 0x01, value);
    }

    public int Id
    {
        get => _memory.Read8(_offset + 0x00);
        set => _memory.Write8(_offset + 0x00, value);
    }

    public override string ToString()
    {
        return $"Id: {Id:x2}, Color: {Color:x2}";
    }
}