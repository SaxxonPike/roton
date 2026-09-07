using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperWorld(
    IMemory memory,
    IKeyList keys,
    IFlags flags)
    : IWorld
{
    private Word _torchCycles;
    private Word _torches;

    public ref Word Ammo =>
        ref memory.GetRef<Word>(0x784C);

    public ref Word BoardIndex =>
        ref memory.GetRef<Word>(0x7859);

    public ref Word EnergyCycles =>
        ref memory.GetRef<Word>(0x785D);

    public IFlags Flags { get; } = flags;

    public ref Word Gems =>
        ref memory.GetRef<Word>(0x784E);

    public ref Word Health =>
        ref memory.GetRef<Word>(0x7857);

    public ref Bool IsLocked =>
        ref memory.GetRef<Bool>(0x79CC);

    public IKeyList Keys { get; } = keys;

    public string Name
    {
        get => memory.ReadString(0x7863);
        set => memory.WriteString(0x7863, value);
    }

    public ref Word Score =>
        ref memory.GetRef<Word>(0x7861);

    public ref Word Stones =>
        ref memory.GetRef<Word>(0x79CD);

    public ref Word TimePassed =>
        ref memory.GetRef<Word>(0x79C8);

    public ref Word TorchCycles =>
        ref _torchCycles;

    public ref Word Torches =>
        ref _torches;

    public int WorldType =>
        -2;

    public override string ToString() =>
        Name;
}