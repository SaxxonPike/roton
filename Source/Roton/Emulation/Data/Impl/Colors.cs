namespace Roton.Emulation.Data.Impl;

public abstract class Colors : FixedStringList, IColors
{
    protected Colors(IMemory memory, int offset) 
        : base(memory, offset)
    {
    }

    protected override int ItemLength => 9;
}