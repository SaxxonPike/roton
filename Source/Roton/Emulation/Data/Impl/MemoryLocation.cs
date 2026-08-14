namespace Roton.Emulation.Data.Impl;

public sealed class MemoryLocation : IXyPair
{
    private readonly IMemory _memory;
    private readonly int _offset;

    internal MemoryLocation(IMemory memory, int offset)
    {
        _memory = memory;
        _offset = offset;
    }

    public IXyPair Clone()
    {
        return new Location(X, Y);
    }

    public int X
    {
        get => _memory.Read8(_offset + 0x00);
        set => _memory.Write8(_offset + 0x00, value);
    }

    public int Y
    {
        get => _memory.Read8(_offset + 0x01);
        set => _memory.Write8(_offset + 0x01, value);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}