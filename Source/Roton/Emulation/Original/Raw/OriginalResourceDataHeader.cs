using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x0052A)]
internal struct OriginalResourceDataHeader
{
    [FieldOffset(0x0000)] public Word EntryCount;
    [FieldOffset(0x0002)] public OriginalString50 Name; // array[24]
    [FieldOffset(0x04CA)] public DWord FileOffset; // array[24]
}