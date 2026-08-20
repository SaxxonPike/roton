using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperExits(IMemory memory) : IExits
{
    public int this[int index]
    {
        get => memory.Read8(index + 0x7768);
        set => memory.Write8(index + 0x7768, value);
    }

    public int East
    {
        get => memory.Read8(0x776B);
        set => memory.Write8(0x776B, value);
    }

    public int North
    {
        get => memory.Read8(0x7768);
        set => memory.Write8(0x7768, value);
    }

    public int South
    {
        get => memory.Read8(0x7769);
        set => memory.Write8(0x7769, value);
    }

    public int West
    {
        get => memory.Read8(0x776A);
        set => memory.Write8(0x776A, value);
    }
}