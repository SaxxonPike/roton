using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

/// <summary>
/// Base class for fixed-length lists that contain fixed-length strings.
/// </summary>
/// <param name="memory">
/// Memory block.
/// </param>
/// <param name="offset">
/// Offset within the memory block of item 0.
/// </param>
public abstract class FixedStringList(IMemory memory, int offset) : FixedList<string>
{
    /// <summary>
    /// Length of the fixed-length strings found in the list.
    /// </summary>
    protected abstract int ItemLength { get; }

    public override void Clear()
    {
        for (var i = 0; i < Count; i++)
            this[i] = string.Empty;
    }

    public override bool Contains(string item) =>
        Contains(item.AsSpan());

    /// <inheritdoc cref="Contains(string)" />
    public virtual bool Contains(ReadOnlySpan<char> item)
    {
        for (var i = 0; i < Count; i++)
            if (EqualsItem(i + FirstIndex, item))
                return true;

        return false;
    }

    public override int IndexOf(string item) =>
        IndexOf(item.AsSpan());

    /// <inheritdoc cref="IndexOf(string)" />
    public virtual int IndexOf(ReadOnlySpan<char> item)
    {
        for (var i = 0; i < Count; i++)
            if (EqualsItem(i + FirstIndex, item))
                return i + FirstIndex;

        return -1;
    }

    protected override string GetItem(int index) =>
        memory.ReadString(offset + index * ItemLength);

    protected virtual ReadOnlySpan<byte> GetItemSpan(int index) =>
        memory.ReadStringSpan(offset + index * ItemLength);

    protected override void SetItem(int index, string value) =>
        SetItem(index, value.AsSpan());

    protected virtual void SetItem(int index, ReadOnlySpan<char> value) =>
        memory.WriteString(offset + index * ItemLength, value);

    protected override bool EqualsItem(int index, string value) =>
        EqualsItem(index, value.AsSpan());

    protected virtual bool EqualsItem(int index, ReadOnlySpan<char> value) =>
        Cp437.CharsEqualBytes(value, GetItemSpan(index));
}