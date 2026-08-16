using System.Diagnostics;

namespace Roton.Emulation.Data.Impl;

public abstract class KeyList(IMemory memory, int offset) : FixedList<bool>, IKeyList
{
    private IMemory Memory
    {
        [DebuggerStepThrough] get => memory;
    }

    public override int Count => 7;

    public override void Clear()
    {
        for (var i = 0; i < Count; i++)
            this[i] = false;
    }

    protected override bool GetItem(int index) 
        => Memory.ReadBool(offset + index);

    protected override void SetItem(int index, bool value) 
        => Memory.WriteBool(offset + index, value);
}