namespace Roton.Emulation.Data.Impl;

public abstract class Colors(IMemory memory, int offset) : FixedStringList(memory, offset), IColors
{
    protected override int ItemLength => 9;
}