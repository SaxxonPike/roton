using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalBoard : IBoard
{
    private readonly IMemory _memory;

    public OriginalBoard(IMemory memory)
    {
        _memory = memory;
        Entrance = new MemoryLocation(_memory, 0x45A9);
        Exits = new OriginalExits(_memory);
    }

    public IXyPair Camera { get; } = new Location();

    public IXyPair Entrance { get; }

    public IExits Exits { get; }
    
    public bool IsDark
    {
        get => _memory.ReadBool(0x4568);
        set => _memory.WriteBool(0x4568, value);
    }

    public int MaximumShots
    {
        get => _memory.Read8(0x4567);
        set => _memory.Write8(0x4567, value);
    }

    public string Name
    {
        get => _memory.ReadString(0x2486);
        set => _memory.WriteString(0x2486, value);
    }

    public bool RestartOnZap
    {
        get => _memory.ReadBool(0x456D);
        set => _memory.WriteBool(0x456D, value);
    }

    public int TimeLimit
    {
        get => _memory.FastRead16(0x45AB);
        set => _memory.FastWrite16(0x45AB, value);
    }
}