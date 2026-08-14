namespace Roton.Emulation.Data.Impl;

public sealed class Int8List(IMemory memory, int offset, int length) : FixedList<int>
{
    public override int Count { get; } = length;
    protected override int GetItem(int index) => unchecked((byte)memory.Read8(offset + index));
    protected override void SetItem(int index, int value) => memory.Write8(offset + index, value);
}