using System;

namespace Roton.Emulation.Data.Impl;

internal sealed class Actor(
    IMemory memory,
    ICodeHeap heap,
    int offset,
    int totalLength)
    : IActor
{
    public int Offset => offset;

    public ref Word Cycle =>
        ref memory.GetRef<Word>(Offset + 0x06);

    public ref Word Follower =>
        ref memory.GetRef<Word>(Offset + 0x0B);

    public ref Word Leader =>
        ref memory.GetRef<Word>(Offset + 0x0D);

    public ref Word Length =>
        ref memory.GetRef<Word>(Offset + 0x17);

    public ref Location Location =>
        ref memory.GetRef<Location>(Offset + 0x00);

    public ref HWord P1 =>
        ref memory.GetRef<HWord>(Offset + 0x08);

    public ref HWord P2 =>
        ref memory.GetRef<HWord>(Offset + 0x09);

    public ref HWord P3 =>
        ref memory.GetRef<HWord>(Offset + 0x0A);

    public ref DWord Pointer =>
        ref memory.GetRef<DWord>(Offset + 0x11);

    public ref Tile UnderTile =>
        ref memory.GetRef<Tile>(Offset + 0x0F);

    public ref Vector Vector =>
        ref memory.GetRef<Vector>(Offset + 0x02);

    public ref Word Instruction =>
        ref memory.GetRef<Word>(Offset + 0x15);

    public Span<HWord> Reserved =>
        memory.GetSpan<HWord>(Offset + 0x19, totalLength - 0x19);
    
    public Span<byte> Raw =>
        memory.GetSpan<byte>(Offset, totalLength);

    public Span<char> Code
    {
        get => heap[Pointer];
        set { }
    }

    public override string ToString()
    {
        var name = ReadOnlySpan<char>.Empty;
        var data = Code;

        if (!data.IsEmpty)
        {
            // walk the code to get the name
            if (data[0] == 0x40)
            {
                var length = data.Length;
                for (var i = 1; i < length; i++)
                {
                    if (data[i] == 0x0D)
                    {
                        name = data.Slice(1, i - 1);
                        break;
                    }
                }
            }

            name = name.IsEmpty ? string.Empty : $" {name.ToString()}";
        }

        return $"{Location}{name.ToString()}";
    }
}