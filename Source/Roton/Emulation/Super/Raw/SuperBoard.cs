using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x4BD5)]
internal struct SuperBoard
{
    [FieldOffset(0x0000)] public SuperSmallString Name;
    [FieldOffset(0x003D)] public SuperTile Tiles; // array[8036]
    [FieldOffset(0x3F05)] public Word StatCount;
    [FieldOffset(0x3F07)] public SuperStat Stats; // array[130]
    [FieldOffset(0x4BB9)] public SuperBoardInfo Info;
}