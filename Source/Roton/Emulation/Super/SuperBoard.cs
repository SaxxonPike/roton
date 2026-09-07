using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperBoard(IMemory memory) : IBoard
{
    private Bool _isDark;

    public ref Location16 Camera =>
        ref memory.GetRef<Location16>(0x776F);

    public ref Location Entrance =>
        ref memory.GetRef<Location>(0x776D);

    public ref Bool IsDark =>
        ref _isDark;

    public ref Word MaximumShots =>
        ref memory.GetRef<Word>(0x7767);

    public string Name
    {
        get => memory.ReadString(0x2BAE);
        set => memory.WriteString(0x2BAE, value);
    }

    public ref Bool RestartOnZap =>
        ref memory.GetRef<Bool>(0x776C);

    public ref Word TimeLimit =>
        ref memory.GetRef<Word>(0x7773);
}