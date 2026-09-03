using System;

namespace Roton.Emulation.Data.Impl;

internal sealed class Actor : IActor
{
    private readonly IMemory _memory;
    private readonly ICodeHeap _heap;

    internal Actor(IMemory memory, ICodeHeap heap, int offset)
    {
        _memory = memory;
        _heap = heap;
        Offset = offset;
    }

    public int Offset { get; }

    public ref Word Cycle => ref _memory.GetRef<Word>(Offset + 0x06);

    public ref Word Follower => ref _memory.GetRef<Word>(Offset + 0x0B);

    public ref Word Leader => ref _memory.GetRef<Word>(Offset + 0x0D);

    public ref Word Length => ref _memory.GetRef<Word>(Offset + 0x17);

    public ref Location Location => ref _memory.GetRef<Location>(Offset + 0x00);

    public ref HWord P1 => ref _memory.GetRef<HWord>(Offset + 0x08);

    public ref HWord P2 => ref _memory.GetRef<HWord>(Offset + 0x09);

    public ref HWord P3 => ref _memory.GetRef<HWord>(Offset + 0x0A);

    public ref DWord Pointer => ref _memory.GetRef<DWord>(Offset + 0x11);

    public ref Tile UnderTile => ref _memory.GetRef<Tile>(Offset + 0x0F);

    public ref Vector Vector => ref _memory.GetRef<Vector>(Offset + 0x02);

    public ref Word Instruction => ref _memory.GetRef<Word>(Offset + 0x15);

    public Span<char> Code
    {
        get => _heap[Pointer];
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