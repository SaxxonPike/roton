using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public sealed class Int16List(IMemory memory, int offset, int count) : FixedList<int>
{
    public override int Count { get; } = count;

    protected override int GetItem(int index)
    {
        return memory.Read16(offset + (index << 1));
    }

    protected override void SetItem(int index, int value)
    {
        memory.Write16(offset + (index << 1), value);
    }
}