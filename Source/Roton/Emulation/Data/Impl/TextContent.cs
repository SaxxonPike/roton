using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Data.Impl;

// TODO: This lives in memory *somewhere*, figure out where
public abstract class TextContent(IMemory memory, int offset)
    : FixedStringList(memory, offset), ITextContent
{
    private int _count;

    protected abstract int Capacity { get; }

    public override int Count => _count;

    public void SetText(IEnumerable<string> content)
    {
        _count = 0;
        foreach (var line in content ?? [])
            SetItem(_count++, line);
    }

    public override void Add(string item)
    {
        if (_count < Capacity)
            SetItem(_count++, item);
    }

    public override void Clear()
    {
        _count = 0;
    }
}