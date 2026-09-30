using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x19)]
internal struct RawStat
{
    [FieldOffset(0x0000)] public Location Location;
    [FieldOffset(0x0002)] public Vector Vector;
    [FieldOffset(0x0006)] public Word Cycle;
    [FieldOffset(0x0008)] public HWord P1;
    [FieldOffset(0x0009)] public HWord P2;
    [FieldOffset(0x000A)] public HWord P3;
    [FieldOffset(0x000B)] public Word Follower;
    [FieldOffset(0x000D)] public Word Leader;
    [FieldOffset(0x000F)] public Tile Under;
    [FieldOffset(0x0011)] public DWord Pointer;
    [FieldOffset(0x0015)] public Word DataPos;
    [FieldOffset(0x0017)] public Word DataLen;
}