using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperBoard : IBoard
{
    private readonly IMemory _memory;
    private Bool _isDark;

    public SuperBoard(IMemory memory)
    {
        _memory = memory;
        Exits = new SuperExits(_memory);
    }

    public ref Location16 Camera => ref _memory.GetRef<Location16>(0x776F);

    public ref Location Entrance => ref _memory.GetRef<Location>(0x776D);

    public IExits Exits { get; }

    public ref Bool IsDark => ref _isDark;

    public ref Word MaximumShots => ref _memory.GetRef<Word>(0x7767);

    public string Name
    {
        get => _memory.ReadString(0x2BAE);
        set => _memory.WriteString(0x2BAE, value);
    }

    public ref Bool RestartOnZap => ref _memory.GetRef<Bool>(0x776C);

    public ref Word TimeLimit => ref _memory.GetRef<Word>(0x7773);
}