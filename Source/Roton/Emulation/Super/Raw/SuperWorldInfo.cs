using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x0018F)]
internal struct SuperWorldInfo
{
    [FieldOffset(0x0000)] public Word Ammo;
    [FieldOffset(0x0002)] public Word Gems;
    [FieldOffset(0x0004)] public HWord Keys; // array[7]
    [FieldOffset(0x000B)] public Word Health;
    [FieldOffset(0x000D)] public Word CurrentBoard;
    [FieldOffset(0x000F)] public Word Unk1;
    [FieldOffset(0x0011)] public Word EnergizerTicks;
    [FieldOffset(0x0013)] public Word Unk2;
    [FieldOffset(0x0015)] public Word Score;
    [FieldOffset(0x0017)] public SuperString20 Name;
    [FieldOffset(0x002C)] public SuperString20 Flags; // array[16]
    [FieldOffset(0x017C)] public Word BoardTimeSec;
    [FieldOffset(0x017E)] public Word BoardTimeHSec;
    [FieldOffset(0x0180)] public HWord IsSave;
    [FieldOffset(0x0181)] public Word StonesOfPower;
    [FieldOffset(0x0183)] public HWord UnkPad; // array[12]
}