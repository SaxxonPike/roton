using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperBoard : IBoard
{
    private readonly IMemory _memory;

    public SuperBoard(IMemory memory)
    {
        _memory = memory;
        Camera = new MemoryLocation16(_memory, 0x776F);
        Entrance = new MemoryLocation(_memory, 0x776D);
        Exits = new SuperExits(_memory);
    }

    public IXyPair Camera { get; }

    public IXyPair Entrance { get; }
    
    public IExits Exits { get; }
    
    public bool IsDark
    {
        get => false;
        set { }
    }

    public int MaximumShots
    {
        get => _memory.Read8(0x7767);
        set => _memory.Write8(0x7767, value);
    }

    public string Name
    {
        get => _memory.ReadString(0x2BAE);
        set => _memory.WriteString(0x2BAE, value);
    }

    public bool RestartOnZap
    {
        get => _memory.ReadBool(0x776C);
        set => _memory.WriteBool(0x776C, value);
    }

    public int TimeLimit
    {
        get => _memory.FastRead16(0x7773);
        set => _memory.FastWrite16(0x7773, value);
    }
}