using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public class OriginalExits(IMemory memory) : IExits
{
    public int this[int index]
    {
        get => memory.Read8(0x4569 + index);
        set => memory.Write8(0x4569 + index, value);
    }

    public int East
    {
        get => memory.Read8(0x456C);
        set => memory.Write8(0x456C, value);
    }

    public int North
    {
        get => memory.Read8(0x4569);
        set => memory.Write8(0x4569, value);
    }

    public int South
    {
        get => memory.Read8(0x456A);
        set => memory.Write8(0x456A, value);
    }

    public int West
    {
        get => memory.Read8(0x456B);
        set => memory.Write8(0x456B, value);
    }

}