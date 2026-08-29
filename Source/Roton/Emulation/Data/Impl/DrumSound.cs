namespace Roton.Emulation.Data.Impl;

internal sealed class DrumSound : FixedList<int>, IDrumSound
{
    private readonly IMemory _memory;
    private readonly int _offset;

    internal DrumSound(IMemory memory, int offset, int index)
    {
        _memory = memory;
        _offset = offset;
        Index = index;
    }

    public int Index { get; }

    public override int Count => _memory.Read16(_offset);

    protected override int GetItem(int index)
    {
        return _memory.Read16(_offset + ((1 + index) << 1));
    }
}