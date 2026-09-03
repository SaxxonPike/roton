namespace Roton.Emulation.Data.Impl;

internal sealed class Timer : ITimer
{
    private readonly IMemory _memory;
    private readonly int _offset;

    internal Timer(IMemory memory, int offset)
    {
        _memory = memory;
        _offset = offset;
    }

    public int Ticks
    {
        get => _memory.Read16(_offset);
        set => _memory.Write16(_offset, value);
    }
}