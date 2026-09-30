using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x061A)]
internal struct SuperResourceDataHeader
{
    [FieldOffset(0x0000)] public Word EntryCount;
    [FieldOffset(0x0002)] public SuperSmallString Name; // array[24]
    [FieldOffset(0x05BA)] public DWord FileOffset; // array[24]
}