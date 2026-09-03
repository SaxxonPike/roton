using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalWorld(
    IMemory memory,
    IKeyList keys,
    IFlags flags)
    : IWorld
{
    private Word _stones;

    public ref Word Ammo => ref memory.GetRef<Word>(0x481E);

    public ref Word BoardIndex => ref memory.GetRef<Word>(0x482B);

    public ref Word EnergyCycles => ref memory.GetRef<Word>(0x4831);

    public ref Word Gems => ref memory.GetRef<Word>(0x4820);

    public ref Word Health => ref memory.GetRef<Word>(0x4829);

    public ref Bool IsLocked => ref memory.GetRef<Bool>(0x4922);

    public IFlags Flags { get; } = flags;

    public IKeyList Keys { get; } = keys;

    public string Name
    {
        get => memory.ReadString(0x4837);
        set => memory.WriteString(0x4837, value);
    }

    public ref Word Score => ref memory.GetRef<Word>(0x4835);

    public ref Word Stones => ref _stones;

    public ref Word TimePassed => ref memory.GetRef<Word>(0x491E);

    public ref Word TorchCycles => ref memory.GetRef<Word>(0x482F);

    public ref Word Torches => ref memory.GetRef<Word>(0x482D);

    public int WorldType => -1;

    public override string ToString() => Name;
}