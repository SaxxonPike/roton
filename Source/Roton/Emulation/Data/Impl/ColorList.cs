using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class ColorList(IMemory memory, int offset) : FixedStringList(memory, offset), IColorList
{
    protected override int ItemLength => 9;

    protected override int FirstIndex => 1;

    protected override bool EqualsItem(int index, ReadOnlySpan<char> value) => 
        GetItemSpan(index).CaseInsensitiveCharacterEqual(value);

    protected override bool EqualsItem(int index, ReadOnlySpan<char> value, Span<byte> buffer) =>
        GetItemSpan(index, buffer).CaseInsensitiveCharacterEqual(value);
}