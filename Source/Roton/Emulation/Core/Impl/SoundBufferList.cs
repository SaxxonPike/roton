using System;
using System.Diagnostics;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core.Impl;

public sealed class SoundBufferList : FixedList<int>, ISoundBufferList
{
    private readonly Lazy<IMemory> _memory;
    private readonly int _offset;

    internal SoundBufferList(Lazy<IMemory> memory, int offset)
    {
        _memory = memory;
        _offset = offset;
    }

    private IMemory Memory
    {
        [DebuggerStepThrough] get => _memory.Value;
    }

    public override int Count => Memory.Read8(_offset);

    protected override int GetItem(int index)
    {
        return Memory.Read8(_offset + index + 1);
    }

    protected override void SetItem(int index, int value)
    {
        Memory.Write8(_offset + index + 1, value);
    }

    public void Enqueue(ISound sound, int? offset = null, int? length = null)
    {
        var inLength = length ?? sound.Length;
        var inOffset = offset ?? 0;
            
        var totalLength = inLength + Memory.Read8(_offset);
        if (totalLength >= 255)
            return;

        var sourceIndex = inOffset;
        var targetIndex = Memory.Read8(_offset) + 1 + _offset;
        var remaining = inLength;
        while (remaining-- > 0)
            Memory.Write8(targetIndex++, sound[sourceIndex++]);
        Memory.Write8(_offset, totalLength);
    }

    public ISoundNote Dequeue()
    {
        var remaining = Memory.Read8(_offset);
        if (remaining <= 0)
            throw new Exception("No notes available in queue!");
            
        var result = new SoundNote
        {
            Note = Memory.Read8(_offset + 1),
            Duration = Memory.Read8(_offset + 2)
        };

        remaining -= 2;
        Memory.Write8(_offset, remaining);
            
        for (var i = 0; i < remaining; i++)
            Memory.Write8(_offset + 1 + i, Memory.Read8(_offset + 3 + i));
            
        return result;
    }

    public override void Clear()
    {
        Memory.Write8(_offset, 0);
    }
}