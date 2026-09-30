using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x0001C)]
internal struct SuperBoardInfo
{
    [FieldOffset(0x0000)] public HWord MaxShots;
    [FieldOffset(0x0001)] public HWord NeighborBoards; // array[4]
    [FieldOffset(0x0005)] public Bool ReenterWhenZapped;
    [FieldOffset(0x0006)] public HWord StartPlayerX;
    [FieldOffset(0x0007)] public HWord StartPlayerY;
    [FieldOffset(0x0008)] public Word DrawXOffset;
    [FieldOffset(0x000A)] public Word DrawYOffset;
    [FieldOffset(0x000C)] public Word TimeLimitSec;
    [FieldOffset(0x000E)] public HWord UnkPad; // array[14]
}