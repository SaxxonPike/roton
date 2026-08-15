using System;
using System.Linq;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class Flags(IMemory memory, int offset) : FixedList<string>, IFlags
{
    public override void Add(string item)
    {
        if (Contains(item))
            return;

        var count = Count;
        for (var i = 0; i < count; i++)
        {
            if (!string.IsNullOrEmpty(GetItem(i)))
                continue;

            SetItem(i, item);
            return;
        }
    }

    public override void Clear()
    {
        for (var i = 0; i < Count; i++)
        {
            this[i] = string.Empty;
        }
    }

    public override bool Remove(string item)
    {
        var count = Count;
        for (var i = 0; i < count; i++)
        {
            if (GetItem(i) != item)
                continue;

            SetItem(i, string.Empty);
            return true;
        }

        return false;
    }

    public string StoneText
    {
        get
        {
            foreach (var flag in this.Select(f => f.ToUpperInvariant()))
            {
                if (flag.Length > 0 && flag[0] == 'Z')
                {
                    return flag.Substring(1);
                }
            }

            return string.Empty;
        }
    }

    protected override string GetItem(int index) => 
        memory.ReadString(offset + index * 21);

    private ReadOnlySpan<byte> GetItemSpan(int index) =>
        memory.ReadStringSpan(offset + index * 21);

    protected override void SetItem(int index, string value) => 
        memory.WriteString(offset + index * 21, value);
    
    protected override bool EqualsItem(int index, string value) => 
        Cp437.CharsEqualBytes(value, GetItemSpan(index));
}