using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalWorld(
    IMemory memory,
    IKeyList keys,
    IFlagList flags,
    ITimerFactory timerFactory)
    : IWorld
{
    private Word _stones;

    public ref Word Ammo =>
        ref memory.GetRef<Word>(0x4822);

    public ref Word BoardIndex =>
        ref memory.GetRef<Word>(0x482F);

    public ref Word EnergyCycles =>
        ref memory.GetRef<Word>(0x4835);

    public ref Word Gems =>
        ref memory.GetRef<Word>(0x4824);

    public ref Word Health =>
        ref memory.GetRef<Word>(0x482D);

    public ref Bool IsLocked =>
        ref memory.GetRef<Bool>(0x4926);

    public IFlagList Flags { get; } = flags;

    public IKeyList Keys { get; } = keys;

    public string Name
    {
        get => memory.ReadString(0x483B);
        set => memory.WriteString(0x483B, value);
    }

    public ref Word Score =>
        ref memory.GetRef<Word>(0x4839);

    public ref Word Stones =>
        ref _stones;

    public ref Word TimePassed =>
        ref memory.GetRef<Word>(0x4922);

    public ITimer TimeLimitTimer { get; } =
        timerFactory.Create(0x4924);

    public ref Word TorchCycles =>
        ref memory.GetRef<Word>(0x4833);

    public ref Word Torches =>
        ref memory.GetRef<Word>(0x4831);

    public int WorldType =>
        -1;

    public override string ToString() =>
        Name;
}