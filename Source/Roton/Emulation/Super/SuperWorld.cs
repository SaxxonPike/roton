using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperWorld(IMemory memory, IKeyList keyList, IFlags flags) : IWorld
{
    private IMemory Memory { get; } = memory;

    public int Ammo
    {
        get => Memory.FastRead16(0x784C);
        set => Memory.FastWrite16(0x784C, value);
    }

    public int BoardIndex
    {
        get => Memory.FastRead16(0x7859);
        set => Memory.FastWrite16(0x7859, value);
    }

    public int EnergyCycles
    {
        get => Memory.FastRead16(0x785D);
        set => Memory.FastWrite16(0x785D, value);
    }

    public IFlags Flags { get; } = flags;

    public int Gems
    {
        get => Memory.FastRead16(0x784E);
        set => Memory.FastWrite16(0x784E, value);
    }

    public int Health
    {
        get => Memory.FastRead16(0x7857);
        set => Memory.FastWrite16(0x7857, value);
    }

    public bool IsLocked
    {
        get => Memory.ReadBool(0x79CC);
        set => Memory.WriteBool(0x79CC, value);
    }

    public IKeyList Keys { get; } = keyList;

    public string Name
    {
        get => Memory.ReadString(0x7863);
        set => Memory.WriteString(0x7863, value);
    }

    public int Score
    {
        get => Memory.FastRead16(0x7861);
        set => Memory.FastWrite16(0x7861, value);
    }

    public int Stones
    {
        get => Memory.FastRead16(0x79CD);
        set => Memory.FastWrite16(0x79CD, value);
    }

    public int TimePassed
    {
        get => Memory.FastRead16(0x79C8);
        set => Memory.FastWrite16(0x79C8, value);
    }

    public int TorchCycles
    {
        get => 0;
        set { }
    }

    public int Torches
    {
        get => 0;
        set { }
    }

    public int WorldType => -2;

    public override string ToString() => Name ?? base.ToString();
}