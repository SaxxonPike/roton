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

    public ref Word Ticks =>
        ref _memory.GetRef<Word>(_offset);
}