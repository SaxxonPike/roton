namespace Roton.Emulation.Data.Impl;

internal sealed class ProgressAnimation(IMemory memory, int offset) 
    : FixedStringList(memory, offset)
{
    public override int Count => 8;
    protected override int ItemLength => 6;
}