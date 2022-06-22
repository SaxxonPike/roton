namespace Roton.Emulation.Data.Impl;

public sealed class Int8List : FixedList<int>
{
    private readonly IMemory _memory;
    private readonly int _offset;

    public Int8List(IMemory memory, int offset, int length)
    {
        _memory = memory;
        _offset = offset;
        Count = length;
    }

    public override int Count { get; }
    protected override int GetItem(int index) => unchecked((byte)_memory.Read8(_offset + index));
    protected override void SetItem(int index, int value) => _memory.Write8(_offset + index, value);
}