using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class FixedStringList(IMemory memory, int offset) : FixedList<string>
{
    protected abstract int ItemLength { get; }

    public override void Clear()
    {
        for (var i = 0; i < Count; i++)
            this[i] = string.Empty;
    }

    public override bool Contains(string item)
    {
        for (var i = 0; i < Count; i++)
            if (EqualsItem(i, item))
                return true;

        return false;
    }

    protected override string GetItem(int index) => 
        memory.ReadString(offset + index * ItemLength);

    private ReadOnlySpan<byte> GetItemSpan(int index) =>
        memory.ReadStringSpan(offset + index * ItemLength);

    protected override void SetItem(int index, string value) => 
        memory.WriteString(offset + index * ItemLength, value);

    protected override bool EqualsItem(int index, string value) => 
        Cp437.CharsEqualBytes(value, GetItemSpan(index));

}