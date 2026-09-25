using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core.Impl;

internal sealed class SoundBufferList(IMemory memory, int offset) : FixedList<int>, ISoundBufferList
{
    public override int Count => memory.Read8(offset);

    protected override int GetItem(int index) => 
        memory.Read8(offset + index + 1);

    protected override void SetItem(int index, int value) => 
        memory.Write8(offset + index + 1, value);

    public void Enqueue(ReadOnlySpan<byte> sound)
    {
        var inLength = sound.Length;
            
        var totalLength = inLength + memory.Read8(offset);
        if (totalLength >= 255)
            return;

        var sourceIndex = 0;
        var targetIndex = memory.Read8(offset) + 1 + offset;
        var remaining = inLength;
        while (remaining-- > 0)
            memory.Write8(targetIndex++, sound[sourceIndex++]);
        memory.Write8(offset, totalLength);
    }

    public SoundNote Dequeue()
    {
        var remaining = memory.Read8(offset);
        if (remaining < 2)
            throw new RotonException("No notes available in queue!");
            
        var result = new SoundNote(
            note: memory.Read8(offset + 1),
            duration: memory.Read8(offset + 2)
        );

        remaining -= 2;
        memory.Write8(offset, remaining);
            
        for (var i = 0; i < remaining; i++)
            memory.Write8(offset + 1 + i, memory.Read8(offset + 3 + i));
            
        return result;
    }

    public override void Clear()
    {
        memory.Write8(offset, 0);
    }

    protected override bool EqualsItem(int index, int value) => 
        GetItem(index) == value;
}