using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00113)]
internal struct OriginalWorldInfo
{
    [FieldOffset(0x0000)] public Word Ammo;
    [FieldOffset(0x0002)] public Word Gems;
    [FieldOffset(0x0004)] public HWord Keys; // array[7]
    [FieldOffset(0x000B)] public Word Health;
    [FieldOffset(0x000D)] public Word CurrentBoard;
    [FieldOffset(0x000F)] public Word Torches;
    [FieldOffset(0x0011)] public Word TorchTicks;
    [FieldOffset(0x0013)] public Word EnergizerTicks;
    [FieldOffset(0x0015)] public Word Unk1;
    [FieldOffset(0x0017)] public Word Score;
    [FieldOffset(0x0019)] public OriginalString20 Name;
    [FieldOffset(0x002E)] public OriginalString20 Flags; // array[10]
    [FieldOffset(0x0100)] public Word BoardTimeSec;
    [FieldOffset(0x0102)] public Word BoardTimeHSec;
    [FieldOffset(0x0104)] public HWord IsSave;
    [FieldOffset(0x0105)] public HWord UnkPad; // array[14]
}