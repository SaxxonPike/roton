namespace Roton.Emulation.Data.Impl;

public sealed class ProgressAnimation : FixedStringList
{
    public ProgressAnimation(IMemory memory, int offset) 
        : base(memory, offset)
    {
    }

    public override int Count => 8;
    protected override int ItemLength => 6;
}