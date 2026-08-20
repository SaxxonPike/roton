using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalBoard : IBoard
{
    private readonly IMemory _memory;
    private Location16 _camera;

    public OriginalBoard(IMemory memory)
    {
        _memory = memory;
        Exits = new OriginalExits(_memory);
    }

    public ref Location16 Camera => ref _camera;

    public ref Location Entrance => ref _memory.GetRef<Location>(0x45A9);

    public IExits Exits { get; }

    public ref Bool IsDark => ref _memory.GetRef<Bool>(0x4568);

    public ref Word MaximumShots => ref _memory.GetRef<Word>(0x4567);

    public string Name
    {
        get => _memory.ReadString(0x2486);
        set => _memory.WriteString(0x2486, value);
    }

    public ref Bool RestartOnZap => ref _memory.GetRef<Bool>(0x456D);

    public ref Word TimeLimit => ref _memory.GetRef<Word>(0x45AB);
}