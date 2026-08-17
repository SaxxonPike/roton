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

    public override bool Contains(string item) =>
        Contains(item.AsSpan());

    public bool Contains(ReadOnlySpan<char> item)
    {
        for (var i = 0; i < Count; i++)
            if (EqualsItem(i, item))
                return true;

        return false;
    }

    protected override string GetItem(int index) =>
        memory.ReadString(offset + index * ItemLength);

    protected ReadOnlySpan<byte> GetItemSpan(int index) =>
        memory.ReadStringSpan(offset + index * ItemLength);

    protected override void SetItem(int index, string value) =>
        SetItem(index, value.AsSpan());

    protected void SetItem(int index, ReadOnlySpan<char> value) =>
        memory.WriteString(offset + index * ItemLength, value);

    protected override bool EqualsItem(int index, string value) =>
        EqualsItem(index, value.AsSpan());

    protected bool EqualsItem(int index, ReadOnlySpan<char> value) =>
        Cp437.CharsEqualBytes(value, GetItemSpan(index));
}