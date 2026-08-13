namespace Roton.Emulation.Data.Impl;

public abstract class FixedStringList(IMemory memory, int offset) : FixedList<string>
{
    protected abstract int ItemLength { get; }

    public override void Clear()
    {
        for (var i = 0; i < Count; i++)
            this[i] = string.Empty;
    }

    protected override string GetItem(int index)
    {
        return memory.ReadString(offset + index * ItemLength);
    }

    protected override void SetItem(int index, string value)
    {
        memory.WriteString(offset + index * ItemLength, value);
    }
}