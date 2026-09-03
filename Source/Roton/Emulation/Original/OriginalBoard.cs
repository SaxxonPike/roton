using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalBoard(
    IMemory memory)
    : IBoard
{
    private Location16 _camera;

    public ref Location16 Camera => ref _camera;

    public ref Location Entrance => ref memory.GetRef<Location>(0x45A9);

    public ref Bool IsDark => ref memory.GetRef<Bool>(0x4568);

    public ref Word MaximumShots => ref memory.GetRef<Word>(0x4567);

    public string Name
    {
        get => memory.ReadString(0x2486);
        set => memory.WriteString(0x2486, value);
    }

    public ref Bool RestartOnZap => ref memory.GetRef<Bool>(0x456D);

    public ref Word TimeLimit => ref memory.GetRef<Word>(0x45AB);
}