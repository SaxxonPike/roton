using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00019)]
internal struct SuperStat
{
    [FieldOffset(0x0000)] public HWord X;
    [FieldOffset(0x0001)] public HWord Y;
    [FieldOffset(0x0002)] public Word StepX;
    [FieldOffset(0x0004)] public Word StepY;
    [FieldOffset(0x0006)] public Word Cycle;
    [FieldOffset(0x0008)] public HWord P1;
    [FieldOffset(0x0009)] public HWord P2;
    [FieldOffset(0x000A)] public HWord P3;
    [FieldOffset(0x000B)] public Word Follower;
    [FieldOffset(0x000D)] public Word Leader;
    [FieldOffset(0x000F)] public SuperTile Under;
    [FieldOffset(0x0011)] public DWord Pointer;
    [FieldOffset(0x0015)] public Word DataPos;
    [FieldOffset(0x0017)] public Word DataLen;
}