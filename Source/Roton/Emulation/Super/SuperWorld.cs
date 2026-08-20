using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperWorld(IMemory memory, IKeyList keyList, IFlags flags) : IWorld
{
    private Word _torchCycles;
    private Word _torches;
    
    private IMemory Memory { get; } = memory;

    public ref Word Ammo => ref Memory.GetRef<Word>(0x784C);

    public ref Word BoardIndex => ref Memory.GetRef<Word>(0x7859);

    public ref Word EnergyCycles => ref Memory.GetRef<Word>(0x785D);

    public IFlags Flags { get; } = flags;

    public ref Word Gems => ref Memory.GetRef<Word>(0x784E);

    public ref Word Health => ref Memory.GetRef<Word>(0x7857);

    public ref Bool IsLocked => ref Memory.GetRef<Bool>(0x79CC);

    public IKeyList Keys { get; } = keyList;

    public string Name
    {
        get => Memory.ReadString(0x7863);
        set => Memory.WriteString(0x7863, value);
    }

    public ref Word Score => ref Memory.GetRef<Word>(0x7861);

    public ref Word Stones => ref Memory.GetRef<Word>(0x79CD);

    public ref Word TimePassed => ref Memory.GetRef<Word>(0x79C8);

    public ref Word TorchCycles => ref _torchCycles;

    public ref Word Torches => ref _torches;

    public int WorldType => -2;

    public override string ToString() => Name;
}