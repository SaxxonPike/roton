using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00056)]
internal struct OriginalBoardInfo
{
    [FieldOffset(0x0000)] public HWord MaxShots;
    [FieldOffset(0x0001)] public Bool IsDark;
    [FieldOffset(0x0002)] public HWord NeighborBoards;
    [FieldOffset(0x0006)] public Bool ReenterWhenZapped;
    [FieldOffset(0x0007)] public OriginalString58 Message;
    [FieldOffset(0x0042)] public HWord StartPlayerX;
    [FieldOffset(0x0043)] public HWord StartPlayerY;
    [FieldOffset(0x0044)] public Word TimeLimitSec;
    [FieldOffset(0x0046)] public HWord UnkPad; // array[16]
}