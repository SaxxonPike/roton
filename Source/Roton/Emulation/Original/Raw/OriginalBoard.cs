using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x02137)]
internal struct OriginalBoard
{
    [FieldOffset(0x0000)] public OriginalString50 Name;
    [FieldOffset(0x0033)] public OriginalTile Tiles; // array[1674]
    [FieldOffset(0x0D47)] public Word StatCount;
    [FieldOffset(0x0D49)] public OriginalStat Stats; // array[152]
    [FieldOffset(0x20E1)] public OriginalBoardInfo Info;
}