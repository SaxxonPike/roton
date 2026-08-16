using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalWorld(IMemory memory, IKeyList keyList, IFlags flags) : IWorld
{
    public int Ammo
    {
        get => memory.FastRead16(0x481E);
        set => memory.FastWrite16(0x481E, value);
    }

    public int BoardIndex
    {
        get => memory.FastRead16(0x482B);
        set => memory.FastWrite16(0x482B, value);
    }

    public int EnergyCycles
    {
        get => memory.FastRead16(0x4831);
        set => memory.FastWrite16(0x4831, value);
    }

    public int Gems
    {
        get => memory.FastRead16(0x4820);
        set => memory.FastWrite16(0x4820, value);
    }

    public int Health
    {
        get => memory.FastRead16(0x4829);
        set => memory.FastWrite16(0x4829, value);
    }

    public bool IsLocked
    {
        get => memory.ReadBool(0x4922);
        set => memory.WriteBool(0x4922, value);
    }

    public IFlags Flags { get; } = flags;

    public IKeyList Keys { get; } = keyList;

    public string Name
    {
        get => memory.ReadString(0x4837);
        set => memory.WriteString(0x4837, value);
    }

    public int Score
    {
        get => memory.FastRead16(0x4835);
        set => memory.FastWrite16(0x4835, value);
    }

    public int Stones
    {
        get => 0;
        set { }
    }

    public int TimePassed
    {
        get => memory.FastRead16(0x491E);
        set => memory.FastWrite16(0x491E, value);
    }

    public int TorchCycles
    {
        get => memory.FastRead16(0x482F);
        set => memory.FastWrite16(0x482F, value);
    }

    public int Torches
    {
        get => memory.FastRead16(0x482D);
        set => memory.FastWrite16(0x482D, value);
    }

    public int WorldType => -1;

    public override string ToString() => Name ?? base.ToString();
}