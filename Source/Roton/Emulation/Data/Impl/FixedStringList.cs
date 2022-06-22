namespace Roton.Emulation.Data.Impl;

public abstract class FixedStringList : FixedList<string>
{
    private readonly IMemory _memory;
    private readonly int _offset;

    protected FixedStringList(IMemory memory, int offset)
    {
        _memory = memory;
        _offset = offset;
    }
    
    protected abstract int ItemLength { get; }

    public override void Clear()
    {
        for (var i = 0; i < Count; i++)
            this[i] = string.Empty;
    }

    protected override string GetItem(int index)
    {
        return _memory.ReadString(_offset + index * ItemLength);
    }

    protected override void SetItem(int index, string value)
    {
        _memory.WriteString(_offset + index * ItemLength, value);
    }
}