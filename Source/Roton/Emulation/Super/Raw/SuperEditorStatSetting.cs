using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00007)]
internal struct SuperEditorStatSetting
{
    [FieldOffset(0x0000)] public HWord P1;
    [FieldOffset(0x0001)] public HWord P2;
    [FieldOffset(0x0002)] public HWord P3;
    [FieldOffset(0x0003)] public Word StepX;
    [FieldOffset(0x0005)] public Word StepY;
}