using System;

namespace Roton.Emulation.Data.Impl;

public abstract class FixedStringSet(IMemory memory, int offset, bool overCapacityBug) : FixedStringList(memory, offset)
{
    public override void Add(string item) =>
        Add(item.AsSpan());

    public void Add(ReadOnlySpan<char> item)
    {
        var addIndex = -1;
        var count = Count;

        for (var i = 0; i < count; i++)
        {
            if (addIndex < 0 && GetItemSpan(i).Length == 0)
            {
                addIndex = i;
                continue;
            }

            if (EqualsItem(i, item))
                return;
        }

        // This replicates a bug in both engines where, if all flag slots
        // are occupied, the highest slot is overwritten.

        if (overCapacityBug && addIndex < 0)
            addIndex = count - 1;

        if (addIndex >= 0)
            SetItem(addIndex, item);
    }

    public override bool Remove(string item) =>
        Remove(item.AsSpan());

    public bool Remove(ReadOnlySpan<char> item)
    {
        var count = Count;

        for (var i = 0; i < count; i++)
        {
            if (!EqualsItem(i, item))
                continue;

            SetItem(i, string.Empty);
            return true;
        }

        return false;
    }
}